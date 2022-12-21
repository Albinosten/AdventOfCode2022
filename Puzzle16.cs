using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;

namespace AdventOfCode2022
{
	public class Puzzle16 : IPuzzle
	{
		private string real => @"input/Puzzle16.txt";
		private string example => @"input/EXAMPLE.TXT";
		public string source => Example ? this.example : this.real;

		public bool Example => false;
		public bool UseParallell = true;
        public bool DebugOutput => true;
		public static int s_Subcount => 7;

        public int FirstResult => 0;
        public long SecondResult => 0;

		public int Solve()
		{
			var allVertex = this.CreateMap();
			var vertexWithFlow = allVertex
				.Where(x => x.FlowRate > 0)
                .Select(x => x.Name)
				.ToList();
			var map = allVertex.ToDictionary(x => x.Name);
			var distanceMap = new Dictionary<string, Dictionary<string, int>>();
			foreach (var i in allVertex)
			{
				distanceMap.Add(i.Name, new Dictionary<string, int>());
				foreach (var j in allVertex)
				{
					distanceMap[i.Name][j.Name] = this.GetDistance(map[i.Name], map[j.Name], map);
				}
			}

			//example 1651
			var clock = new Stopwatch();
            
			clock.Start();
			var baa = this.Solve(vertexWithFlow, distanceMap, map);
			clock.Stop();
			Console.WriteLine("Answer "+baa+ " took: " +clock.Elapsed+ " with parallell = " + UseParallell + " subcount: "+ s_Subcount);
			
            clock.Reset();
            
			clock.Start();
			this.UseParallell = !this.UseParallell;
			var abb = this.Solve(vertexWithFlow, distanceMap, map);
			clock.Stop();
			Console.WriteLine("Answer "+abb+ " took: " +clock.Elapsed+ " with parallell = " + UseParallell + " subcount: "+ s_Subcount);

			return 0;
		}
        public long SolveNext()
        {
            return 0;
        }
		int Solve(List<string> vertexWithFlow, Dictionary<string, Dictionary<string, int>> distanceMap, Dictionary<string, Vertex> map) 
		{
			string next = "";
			string current = map.Keys.First();
			int result = 0;
			var time = 30;
            var vertex = vertexWithFlow.ToList();
			while (time>0)
			{
				if(vertex.Count() == 0)
				{
					break;
				}
				next = this.UseParallell 
					? this.GetNextWithParallellPermutations(current, vertex, distanceMap, map, time) 
					: this.GetNextWithPermutations(current, vertex, distanceMap, map, time);
				if(DebugOutput)Console.WriteLine(next + " at time:" + (30-time+2) + " with flow of: "+ map[next].FlowRate);
				if(distanceMap[current][next] > 0) 
				{
					time -= distanceMap[current][next];
					time--;
					result += (time) * map[next].FlowRate;
				}
				
				current = next;
				vertex.Remove(next);
			}
			return result;
		}
	
		string GetNextWithParallellPermutations(string start,List<string> vertexWithFlow, Dictionary<string, Dictionary<string, int>> distanceMap, Dictionary<string, Vertex> map, int minutes)
		{
			var usingLinq = vertexWithFlow.Select(x => (0, x))
				.ToList()
				.GetPermutations(GetNumberOfMinSubcount(vertexWithFlow.Count))
				.AsParallel()
                .WithDegreeOfParallelism(512)
				.Max(p =>
				{
					var simulationMinutes = minutes;
					var lastIndex = start;
					var currentValue = 0;
					foreach (var nextVertex in p)
					{
						var distance = distanceMap[lastIndex][nextVertex.Item2];
						lastIndex = nextVertex.Item2;
						simulationMinutes -= distance;
						simulationMinutes--;
						if(simulationMinutes > 0)
						{
							currentValue += map[nextVertex.Item2].FlowRate * simulationMinutes;
						}
					}
				return (currentValue, p.First());
				
			});

			return usingLinq.Item2.x;
		}
        int GetNumberOfMinSubcount(int count)
        {
            return Math.Min(count, s_Subcount);
        }
		string GetNextWithPermutations(string start, List<string> vertexWithFlow, Dictionary<string, Dictionary<string, int>> distanceMap, Dictionary<string, Vertex> map, int minutes)
		{
			var vertexWithFlowNames = vertexWithFlow
                .Select(x => x)
                .ToList();

			var permutations = vertexWithFlowNames.GetPermutations(GetNumberOfMinSubcount(vertexWithFlow.Count));
			var maxValue = 0;
			var bestStart = "";
			foreach (var permutation in permutations)
			{
				var simulationMinutes = minutes;
				var currentValue = 0;
				var lastIndex = start;
				foreach (var nextVertex in permutation)
				{
					var distance = distanceMap[lastIndex][nextVertex];
					lastIndex = nextVertex;
					simulationMinutes -= distance;
					simulationMinutes--;
					if(simulationMinutes > 0)
					{
						currentValue += map[nextVertex].FlowRate * simulationMinutes;
					}
				}
				if (currentValue >= maxValue)
				{
					maxValue = currentValue;
					bestStart = permutation.First();
				}
			}

			return bestStart;
		}
		int GetDistance(Vertex start, Vertex end, Dictionary<string, Vertex> map)
		{
			var q = new List<Vertex>();
			var visited = new List<Vertex>();
			q.Add(start);
			var count = 0;
			while (q.Any())
			{
				var nextBatch = new List<Vertex>();
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

		IList<Vertex> CreateMap()
		{
			var allLines = File.ReadAllLines(source, Encoding.UTF8);
			var result = new List<Vertex>();
			foreach (var line in allLines)
			{
				var a = line.Split('=', ';', ',').ToList();
				var flowRate = int.Parse(a[1]);
				var firstNeighbour = new string(a[2].TakeLast(2).ToArray());
				var lastNeighbours = a.Skip(3).Select(x => $"{x[1]}{x[2]}").ToList();
				var neighbours = new List<string>();
				neighbours.Add(firstNeighbour);
				neighbours.AddRange(lastNeighbours);

				var vertex = new Vertex()
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
	public class Vertex
	{
		public string Name { get; set; }
		public int FlowRate { get; set; }
		public List<string> Neighbours { get; set; }
	}
}
