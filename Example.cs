using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AdventOfCode2022
{
	public class Example : IPuzzle
	{
        public int FirstResult => 0;
        public long SecondResult => 0;
        public static string path => "input/EXAMPLE.TXT";
        // public static string path => "input/Puzzle16.txt";

        public long SolveNext()
        {
            return 0;
        }
		public int Solve() 
		{
			var allVertex = this.CreateMap();
			var vertexWithFlow = allVertex
				.Where(x => x.FlowRate > 0)
				.ToList();
			vertexWithFlow.Add(allVertex[0]);
			var map = allVertex.ToDictionary(x => x.Name);
			var distanceMap = new Dictionary<string, Dictionary<string, int>>();

			foreach(var i in allVertex)
			{
				distanceMap.Add(i.Name, new Dictionary<string, int>());
				foreach(var j in allVertex)
				{
					distanceMap[i.Name][j.Name] = this.GetDistance(map[i.Name], map[j.Name], map);
				}
			}

			var aToB = this.GetDistance(map["AA"], map["BB"], map);
			var aToF = this.GetDistance(map["AA"], map["HH"], map);
			var hToC = this.GetDistance(map["HH"], map["CC"], map);


			var result = this.FloydWarshallWithPathReconstruction(vertexWithFlow, distanceMap);

			foreach (var next in result.next)
			{
				Console.WriteLine("");
				Console.WriteLine("start: " + next.Key);
				foreach (var a in next.Value) 
				{
					Console.WriteLine(a.Value.Name + " " + result.dist[next.Key][a.Value.Name]);
				}
			}
            //62651000000
            for(long i = 0; i< 1307674368000; i++)
            {
                if(i%1000000==0){Console.WriteLine(i);}
            }

			var aToB1 = result.dist["AA"]["BB"];
			var aToF1 = result.dist["AA"]["HH"];
			var hToC1 = result.dist["HH"]["CC"];

			return 0;
		}
		/*
		 Should be 
		DD
		BB
		JJ
		HH
		EE
		CC

		 */
		(Dictionary<string, Dictionary<string, int>> dist, Dictionary<string, Dictionary<string, Vertexx>> next) 
            FloydWarshallWithPathReconstruction(List<Vertexx> vertices, Dictionary<string, Dictionary<string, int>>  distanceMap)
		{
			var dist = new Dictionary<string, Dictionary<string, int>>();
			var next = new Dictionary<string, Dictionary<string, Vertexx>>();
			for (int i = 0; i < vertices.Count; i++) 
			{
				dist.Add(vertices[i].Name, new Dictionary<string, int>());
				next.Add(vertices[i].Name, new Dictionary<string, Vertexx>());
				for (int j = 0; j < vertices.Count; j++)
				{
					dist[vertices[i].Name][vertices[j].Name] = int.MaxValue;
					next[vertices[i].Name][vertices[j].Name] = null;
				}
			}
			for (int u = 0; u < vertices.Count; u++)
			{
				for (int v = 0; v < vertices.Count; v++)
				{
					dist[vertices[u].Name][vertices[v].Name] = distanceMap[vertices[u].Name][vertices[v].Name];
					next[vertices[u].Name][vertices[v].Name] = vertices[v];
				}
			}
			for (int v = 0; v < vertices.Count; v++)
			{
				dist[vertices[v].Name][vertices[v].Name] = 0;
				next[vertices[v].Name][vertices[v].Name] = vertices[v];
			}

			for (int k = 0; k < vertices.Count; k++)
			{
				for (int i = 0; i < vertices.Count; i++)
				{
					for (int j = 0; j < vertices.Count; j++)
					{
						if(dist[vertices[i].Name][vertices[j].Name] > dist[vertices[i].Name][vertices[k].Name] + dist[vertices[k].Name][vertices[j].Name]) 
						{
							dist[vertices[i].Name][vertices[j].Name] = dist[vertices[i].Name][vertices[k].Name] + dist[vertices[k].Name][vertices[j].Name];
							next[vertices[i].Name][vertices[j].Name] = next[vertices[i].Name][vertices[k].Name];
						}
					}
				}
			}
			return (dist, next);
		}
		int GetDistance(Vertexx start, Vertexx end, Dictionary<string, Vertexx> map) 
		{
			var q = new List<Vertexx>();
			var visited = new List<Vertexx>();
			q.Add(start);
			var count = 0;
			while (q.Any()) 
			{
				var nextBatch = new List<Vertexx>();
				foreach (var v in q)
				{
					if (v == end)
					{
						return count;
					}
					visited.Add(v);
					foreach (var n in v.Neighbours)
					{
						var neighbour = map[n];
						if (!visited.Contains(neighbour))
						{
							nextBatch.Add(neighbour);
						}
					}
				}
				q = nextBatch;
				count++;
			}
			return count;
		}

		IList<Vertexx> CreateMap() 
		{
			var allLines = File.ReadAllLines(path, Encoding.UTF8);
			var result = new List<Vertexx>();
			foreach (var line in allLines)
			{
				var a = line.Split('=', ';', ',').ToList();
				var flowRate = int.Parse(a[1]);
				var firstNeighbour = new string(a[2].TakeLast(2).ToArray());
				var lastNeighbours = a.Skip(3).Select(x => $"{x[1]}{x[2]}").ToList();
				var neighbours = new List<string>();
				neighbours.Add(firstNeighbour);
				neighbours.AddRange(lastNeighbours);

				var vertex = new Vertexx()
				{
					Name = $"{line[6]}{line[7]}",
					FlowRate = flowRate,
					Neighbours = neighbours,
				};
				result.Add(vertex);
			}
			return result;
		}
	}
	public class Vertexx
	{
		public string Name { get; set; }
		public int FlowRate { get; set; }
		public List<string> Neighbours { get; set; }
	}
}
