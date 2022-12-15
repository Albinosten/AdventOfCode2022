using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2022
{
    public class Puzzle14 : IPuzzle
    {
        private IList<string> allLines;
        public Puzzle14()
        {
            this.allLines = File.ReadAllLines(path).ToList();
        }
        // public static string path => "input/EXAMPLE.TXT";
        public static string path => "input/Puzzle14.txt";
        public int FirstResult => 1072;
        public long SecondResult => 24659;

        public int Solve()
        {
            return this.Solve(false);
        }
        public long SolveNext()
        { 
            return this.Solve(true);
        }
        int Solve(bool hasFloor)
        {
            var yMax = this.allLines
                .SelectMany(x => x.Split(" -> "))
                .Select(x => int.Parse(x.Split(',').Last()))
                .Max() +2;

            if(hasFloor)
            {
                this.allLines.Add($"{s_startPos.Item1-yMax},{yMax} -> {s_startPos.Item1+yMax},{yMax}");                
            }
            
            var map = new HashSet<(int,int)>();
            foreach(var line in this.allLines)
            {
                var cordinates = new List<(int,int)>();
                foreach(var pos in line.Split(" -> ").Select(x => x.Split(',')))
                {
                    cordinates.Add(new (int.Parse(pos[0]),int.Parse(pos[1])));
                }
                var stoneLine = this.Interpolate(cordinates);
                foreach(var stone in stoneLine)
                {
                    map.Add(stone);
                }
            }
            
            var numberOfSand = hasFloor ? 1 : 0;
            while (TryDropSand(out var sandPos, map, yMax))
            {
                numberOfSand++;
                map.Add(sandPos);
            }
            return numberOfSand;
        }

        static (int,int y) s_startPos => (500,0);
        bool TryDropSand(out (int,int)newPos, HashSet<(int,int)> map, int yMax)
        {
            newPos = s_startPos;
            for(int i = s_startPos.Item2; i <= yMax; i++)
            {
                newPos = (newPos.Item1,newPos.Item2+1);
                if(map.Contains(newPos)) //finns en under?
                {
                    newPos = (newPos.Item1-1,newPos.Item2);
                    if(map.Contains(newPos))//finns till vänster?
                    {
                        newPos = (newPos.Item1+2,newPos.Item2);
                        if(map.Contains(newPos))//finns till höger?
                        {
                            newPos = (newPos.Item1-1,newPos.Item2-1);
                            return newPos != s_startPos;
                        }
                    }
                }
            }
            return false;
        }

        public List<(int,int)> Interpolate (List<(int,int)> input)
        {
            var result = new List<(int,int)>();
            for(int i = 0; i < input.Count -1; i++)
            {
                var first = input[i];
                var second = input[i+1];

                for(int j = Math.Min(first.Item1, second.Item1); j <= Math.Max(first.Item1, second.Item1); j++)
                {
                    result.Add(new (j, first.Item2));
                }
                for(int j = Math.Min(first.Item2, second.Item2); j <= Math.Max(first.Item2, second.Item2); j++)
                {
                    result.Add(new (first.Item1, j));
                }
            }
            return result;
        }   
    }
}