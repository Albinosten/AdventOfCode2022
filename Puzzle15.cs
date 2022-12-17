using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2022
{
    public class Puzzle15 : IPuzzle
    {
        private IList<string> allLines;
        public Puzzle15()
        {
            this.allLines = File.ReadAllLines(path).ToList();
        }
        // public static string path => "input/EXAMPLE.TXT";
        public static string path => "input/Puzzle15.txt";
        public int FirstResult => 5335787;
        public long SecondResult => 13673971349056;

        public int Solve()
        {
            var map = this.CreateMap();
            var maxDistance = map.Values.Max()+1;
            var xMin = map.Keys.Min(x => x.Item1)-maxDistance;
            var xMax = map.Keys.Max(x => x.Item1)+maxDistance;

            var result = this.SolveNext(xMax, xMin, 2000000, map);
            return (xMax - xMin) - result.Item2.Count();
        }

        public long SolveNext()
        {
            var map = this.CreateMap();

            var maxNumber = 4000000;
            for( int y = 0; y<= maxNumber; y++)
            {
                var result = this.SolveNext(maxNumber, 0, y, map);
                if(result.Item1)
                {
                    return (result.Item2[0] * 4000000L) + y;
                }
            }
            throw new Exception("No match found");
        }
        Dictionary<(int,int), int> CreateMap()
        {
            var map = new Dictionary<(int,int), int>();
            foreach(var line in this.allLines)
            {
                var parsed = line.Replace('=',',').Replace(':',',').Split(',');
                var sensor = (GetNumber(parsed,1));
                var becon = (GetNumber(parsed,5));
                var distance = this.GetDistanceBetweenPositions(sensor,becon);
                map[sensor] = distance;
            }
            return map;
        }
        
        (bool, List<int>) SolveNext(int xMax,int xMin, int y, Dictionary<(int,int), int> map)
        {
            var result = new List<int>();
            for(int x = xMin; x <= xMax; x++)
            {
                var beacon = (x,y);
                var canBePlaced = true;
                foreach(var sensor in map)
                {
                    var distance = this.GetDistanceBetweenPositions(beacon, sensor.Key);
                    if(distance <= sensor.Value)
                    {
                        x += sensor.Value - distance;
                        canBePlaced = false;
                        break;
                        //can not place here;
                    }
                }
                if(canBePlaced)
                {
                    result.Add(x);
                }  
            }
            return (result.Count > 0, result);
        }
        (int, int) GetNumber(string[] s, int i)
        {
            return (int.Parse(s[i]), int.Parse(s[i+2]));
        }
        int GetDistanceBetweenPositions((int,int) first,(int,int) second)
        {
            return Math.Abs(first.Item1 - second.Item1) + Math.Abs(first.Item2 - second.Item2);
        }
    }
}