using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2022
{
    public class Puzzle1 : IPuzzle
    {
        private IList<string> allLines;
        public Puzzle1()
        {
            this.allLines = File.ReadAllLines(path);
        }
        public static string path => "input/Puzzle1.txt";
        public int FirstResult => 70116;

        public long SecondResult => 206582;

        public int Solve()
        {
            var max = 0;
            var current =0;
            foreach(var line in allLines)
            {
                if(!string.IsNullOrEmpty(line))
                {
                    current += int.Parse(line);
                }
                else 
                {
                    if(current> max)
                    {
                        max = current;
                    }
                    current = 0;
                }
            }
            return max;

        }

        public long SolveNext()
        {
            var result = new List<int>();
            var current = 0;
            foreach(var line in allLines)
            {
                if(!string.IsNullOrEmpty(line))
                {
                    current += int.Parse(line);
                }
                else 
                {
                    result.Add(current);
                    current = 0;
                }
                
            }
            return result
                .OrderByDescending(x => x)
                .Take(3)
                .Sum();
        }
    }
}