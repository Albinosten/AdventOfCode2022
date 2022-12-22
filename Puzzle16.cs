using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AdventOfCode2022
{
	public class Puzzle16 : IPuzzle
	{
		private string real => @"input/Puzzle16.txt";
		private string example => @"input/EXAMPLE.TXT";
		public string source => Example ? this.example : this.real;

        public bool Example => false;
        public bool UseBFS => false;
		public int Subcount = 4;

        public int FirstResult => Example ? 1651 : 1915;
        public long SecondResult => Example ? 1707 : 2772;

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
            return this.UseBFS
                ? this.Solver_BFS(vertexWithFlow, distanceMap, map, 30)
                : this.Solve(vertexWithFlow, distanceMap, map);
		}

        public long SolveNext()
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
            if(this.UseBFS)
            {
                return this.Solver_BFS(vertexWithFlow, distanceMap, map, 26, 0, true);
            }

            var me = this.Solve(vertexWithFlow, distanceMap, map, 26);
            this.Subcount = 6;
            var elephant = this.Solve(vertexWithFlow, distanceMap,map, 26);
            return me+elephant;
        }
        public int Solver_BFS(List<string> vertexWithFlow
            , Dictionary<string, Dictionary<string, int>> distanceMap
            , Dictionary<string, Vertex> map
            , int time
            , int score = 0
            , bool useElephant = false
            )
        {
            var q = new Queue<(string currentvalve, int timeRemaining, int totalRelief, List<string> openValves)>();
            q.Enqueue(("AA", time, score, new List<string>()));

            int maxValue = 0;
            while(q.Any())
            {
                var c = q.Dequeue();
                foreach(var edge in vertexWithFlow)
                {
                    var distance = distanceMap[c.currentvalve][edge] + 1;
                    if( distance < c.timeRemaining && !c.openValves.Contains(edge))
                    {
                        var presure = (c.timeRemaining - distance) * map[edge].FlowRate;
                        var openedValves = c.openValves.ToList();
                        openedValves.Add(edge);
                        q.Enqueue((edge, c.timeRemaining-distance , c.totalRelief + presure, openedValves));
                        if(c.totalRelief + presure > maxValue)
                        {
                            maxValue = c.totalRelief + presure;
                        }
                    }
                    else if (useElephant && c.totalRelief >= maxValue/2)
                    {
                        var a = Solver_BFS(vertexWithFlow.Except(c.openValves).ToList()
                            , distanceMap
                            , map
                            , time
                            , c.totalRelief
                            , false
                            );
                        maxValue = Math.Max(a, maxValue);
                        
                    }
                }
            }

            return maxValue;
        }

		int Solve(List<string> vertex
            , Dictionary<string, Dictionary<string, int>> distanceMap
            , Dictionary<string, Vertex> map
            , int startTime = 30
            )
		{
			string current = "AA";
			int result = 0;
            var time = startTime;
			while (time > 0)
			{
				if(vertex.Count() == 0)
				{
					break;
				}
				var next = this.GetNextWithPermutations(current, vertex, distanceMap, map, time);

                var distance = distanceMap[current][next] + 1;
                time -= distance;
                if(time > 0)
				{
					result += (time) * map[next].FlowRate;
                    current = next;
                    vertex.Remove(next);
				}
			}
			return result;
		}
        int GetNumberOfMinSubcount(int count)
        {
            return Math.Min(count, Subcount);
        }
		string GetNextWithPermutations(string start, List<string> vertexWithFlow, Dictionary<string, Dictionary<string, int>> distanceMap, Dictionary<string, Vertex> map, int minutes, bool elephant = false)
		{
			var permutations = vertexWithFlow.GetPermutations(GetNumberOfMinSubcount(vertexWithFlow.Count));
			var maxValue = 0;
			var bestPermutation = "";
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
					bestPermutation = permutation.First();
				}
			}

			return bestPermutation;
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
