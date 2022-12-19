using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2022
{
    public class Puzzle16 : IPuzzle
    {
        private IList<string> allLines;
        public Puzzle16()
        {
            this.allLines = File.ReadAllLines(path);
        }
        // public static string path => "input/EXAMPLE.TXT";
        public static string path => "input/Puzzle16.txt";
        public int FirstResult => 0;
        public long SecondResult => 0;
        private Dictionary<string, Dictionary<string,int>> distanceMap;

        public int Solve()
        {
            var map = this.GetMap();
            var vertexWithFlow = map.Values
                .Where(x => x.FlowRate > 0)
                .ToList();
            this.distanceMap = new Dictionary<string, Dictionary<string, int>>();
            foreach(var start in map.Values)
            {
                distanceMap.Add(start.Name, new Dictionary<string, int>());
                foreach(var end in map.Values)
                {
                    var distance = this.GetDistanceToNode(map, start,end);
                    distanceMap[start.Name][end.Name] = distance;
                }
            }

            
            // });
            
            //1651 for exampl
            //1709 too low
            //2115 too high
            return this.Search(map.First().Value, vertexWithFlow, (30,0));
            // return this.TrySolveWithPermutations(vertexWithFlow, distanceMap, map);
            // return this.BFS_Improved(vertexWithFlow.ToDictionary(x => x.Name)
            //     , map.Values.First()
            //     , distanceMap
            //     , vertexWithFlow
            //     );
        }

            (int Time, int Score) Move(Vertex from, Vertex to, (int Time, int Score) previous)
            {
                var dist = this.distanceMap[from.Name][to.Name];

                var tEnd = previous.Time - dist - 1;
                return (tEnd, previous.Score + (tEnd * to.FlowRate ));
            }
            int Search(Vertex from, List<Vertex> too, (int Time, int Score) previous)
            {
                var highScore = 0;
                foreach (var to in too)
                {
                    var move = Move(from, to, previous);
                    if (move.Time > 0)
                    {
                        if (move.Score > highScore)
                        {
                            highScore = move.Score;
                        }
                        if (too.Count > 1)
                        {
                            var a = Search(to, too.Where(j => j != to).ToList(), move);
                            highScore = a > highScore ? a : highScore;
                        }
                    }
                }


                return highScore;
            }
        int SolveRecirsive(Vertex start, List<Vertex> too, Dictionary<string, Dictionary<string,int>> distanceMap)
        {
            

            return 0;
        }

int TrySolveWithPermutations(List<Vertex> vertexWithFlow, Dictionary<string, Dictionary<string,int>> distanceMap, Dictionary<string, Vertex> map)
{
int maxValue = 0;
            var bestPermutation = new List<string>();
            var vertexWithFlowNames = vertexWithFlow.OrderByDescending(x => x.FlowRate).Select(x => x.Name).Take(10).ToList();
            var permutations = vertexWithFlowNames.GetPermutations(vertexWithFlowNames.Count());
            long count = 0;
            long outputFreq = 100000L;
            // Parallel.ForEach(permutations, new ParallelOptions { MaxDegreeOfParallelism = 100 }, permutation =>  
            // {
                var lastIndex = map.First().Key;
                var currentValue = 0;
                var minutes = 30;
            foreach(var permutation in permutations)
            {
                count++;
                currentValue = 0;
                minutes = 30;
                if(count % outputFreq == 0)
                {
                    Console.WriteLine(count);
                }

                foreach(var nextVertex in permutation)
                {
                    var distance = distanceMap[lastIndex][nextVertex];
                    lastIndex = nextVertex;
                    minutes -= distance;
                    minutes--;
                    if(minutes>0)
                    {
                        currentValue += map[nextVertex].FlowRate * minutes;
                    }
                }
                if(currentValue > maxValue)
                {
                    maxValue = currentValue;
                    bestPermutation = permutation.ToList();
                }
            }
            return maxValue;
}

        
/*
//expected result
DD 20   1 step 
BB 13   2 steps
JJ 21   3 steps
HH 22   7 steps
EE 3    4 steps
CC 2    3 steps

//actual
DD 20
JJ 21
HH 22
BB 13
EE 3
CC 2

AA => DD
DD => cc => BB
BB =>  AA => II  => JJ
jj => II => AA => DD => EE => FF => GG => HH 
HH => GG => FF => EE
EE => DD => CC
*/
        int BFS_Improved(Dictionary<string,Vertex> map
            , Vertex root
            , Dictionary<string, Dictionary<string, int>> distanceMap
            , List<Vertex> allWithFlow
            )
        {
            var q = new List<Vertex>();
            root.Discovered = true;
            q.Add(root);
            var i = 0;
            var result = 0;
            Vertex last = root;
            while(q.Any() && i < 30)
            {
                var a = this.GetSingleBest(last, q, distanceMap);
                var v = a.Item1;
                q = a.q;
                i += a.distance;
                if(v.FlowRate > 0)
                {
                    i++;
                    v.IsOn = true;
                    //Console.WriteLine(v.Name + " " + v.FlowRate);
                }
                // if(v == goal) return 0;

                foreach(var n in allWithFlow)
                {
                    var w = n;
                    if(!w.Discovered)
                    {
                        w.Discovered = true;
                        w.Parent = v;
                        q.Add(w);
                    }
                }
            }
            return result;
        }
        List<(Vertex,int score)> GetBest(Vertex last, List<Vertex> q, Dictionary<string, Dictionary<string, int>> distanceMap)
        {
            var score = new List<(Vertex, int)>();
            foreach(Vertex i in q)
            {
                var distance = distanceMap[last.Name][i.Name];
                score.Add(new (i, i.FlowRate - distance));
            }
            return score;
        }
        (Vertex v, int distance, int score, List<Vertex> q) GetSingleBest(Vertex last, List<Vertex> q, Dictionary<string, Dictionary<string, int>> distanceMap)
        {
            var bestList = this.GetBest(last, q, distanceMap).GroupBy(c => c.score);
            var group = bestList.OrderByDescending(x => x.Key).First();


            if(group.Count() == 1)
            {
                var l = q;
                l.Remove(group.First().Item1);
                return (group.First().Item1, distanceMap[last.Name][group.First().Item1.Name], group.First().score, l);
            }

            var a = new List<(Vertex, int distance, int score, List<Vertex>)>();
            foreach(var potentian in group)
            {
                var nextList = q.ToList();
                nextList.Remove(potentian.Item1);
                var b = this.GetSingleBest(potentian.Item1, nextList, distanceMap);
                a.Add(b);
            }

            return a.OrderByDescending(x => x.score).First();
        }


        public long SolveNext()
        { 
            return 0;
        }

        int GetDistanceToNode(Dictionary<string,Vertex> map, Vertex start, Vertex end)
        {
            var visited = new HashSet<Vertex>();
            var next = new HashSet<Vertex>();
            next.Add(start);
            var stepps = 0;
            visited.Add(start);
            while(next.Count > 0)
            {
                var nextBatch = new HashSet<Vertex>();
                foreach (var current in next)
                {
                    if(current == end)
                    {
                        return stepps;
                    }
                    
                    visited.Add(current);
                    foreach (var item in current.Neighbours)
                    {
                        nextBatch.Add(map[item]);
                    }

                }
                next = nextBatch;
                stepps++;
            }
            return stepps;
        }
        
        Dictionary<string,Vertex> GetMap()
        {
            var result = new Dictionary<string,Vertex>();
            foreach(var line in this.allLines)
            {
                var a = line.Split('=',';',',');
                var neighbours = a.Skip(3)
                    .Select(x => new String( x.TakeLast(2).ToArray()))
                    .ToList();
                neighbours.Insert(0, new string(a[2].TakeLast(2).ToArray()));

                var vertex = new Vertex()
                {
                    Name = $"{line[6]}{line[7]}",
                    FlowRate = int.Parse(a[1]),
                    Neighbours = neighbours,

                };
                result.Add(vertex.Name, vertex);
            }
            return result;
        }

    }

    public class Vertex
    {
        public string Name {get; set;}
        public List<string> Neighbours {get; set;}
        public int FlowRate {get;set;}
        public bool Discovered{get;set;}
        public bool IsOn {get;set;}
        public Vertex Parent {get;set;}


    }
}