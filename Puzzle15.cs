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
        public long SecondResult => 0;

        public int Solve()
        {
            return this.Solve(2000000);
            // return this.Solve(10);
        }
        public long SolveNext()
        {

            var map = new Dictionary<(int,int), int>();
            var becons = new HashSet<(int,int)>();
            foreach(var line in this.allLines)
            {
                var parsed = line.Replace('=',',').Replace(':',',').Split(',');
                var sensor = (GetNumber(parsed,1));
                var becon = (GetNumber(parsed,5));
                var distance = this.GetDistanceBetweenPositions(sensor,becon);
                map[sensor] = distance;
                becons.Add(becon);
            }


            var maxNumber = 4000000;
            for( int y = 0; y<= maxNumber; y++)
            {
                var result = this.SolveNext(maxNumber,y, map);
                if(result.Item1)
                {
                    return (result.Item2 * 4000000) + y;
                }
            }
            return 0;
            // return this.Solve(0);
        }

        int Solve(int y)
        {
            var map = new Dictionary<(int,int), int>();
            var becons = new HashSet<(int,int)>();
            foreach(var line in this.allLines)
            {
                var parsed = line.Replace('=',',').Replace(':',',').Split(',');
                var sensor = (GetNumber(parsed,1));
                var becon = (GetNumber(parsed,5));
                var distance = this.GetDistanceBetweenPositions(sensor,becon);
                map[sensor] = distance;
                becons.Add(becon);
            }

            var maxDistance = map.Values.Max()+1;
            var xMin = map.Keys.Min(x => x.Item1)-maxDistance;
            var xMax = map.Keys.Max(x => x.Item1)+maxDistance;

            var result = 0;
            for(int x = xMin; x <= xMax; x++)
            {
                var testBecon = (x, y);
                var canNotPlacebecon = false;
                foreach(var sensors in map)
                {
                    var distance = this.GetDistanceBetweenPositions(testBecon, sensors.Key);
                    if(!becons.Contains(testBecon) &&  distance <= sensors.Value)
                    {
                        canNotPlacebecon = true;
                        //can not place here;
                    }
                }
                if(canNotPlacebecon)
                {
                    result++;
                }  
            }
            return result;
        }
        (bool, int) SolveNext(int xMax, int y, Dictionary<(int,int), int> map)
        {


            for(int x = 0; x <= xMax; x++)
            {
                var testBecon = (x, y);
                var canBePlaced = true;
                foreach(var sensors in map)
                {
                    var distance = this.GetDistanceBetweenPositions(testBecon, sensors.Key);
                    if(/*!becons.Contains(testBecon) && */ distance <= sensors.Value)
                    {
                        canBePlaced = false;
                        //can not place here;
                    }
                }
                if(canBePlaced)
                {
                    return (true, x);
                }  
            }
            return (false, 0);
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