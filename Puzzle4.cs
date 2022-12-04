using System.IO;
using System.Collections.Generic;
using System.Linq;
using System;

namespace AdventOfCode2022
{
    public class Puzzle4 : IPuzzle
    {
        private IList<string> allLines;
        public Puzzle4()
        {
            this.allLines = File.ReadAllLines(path);
        }
        public static string path => "input/Puzzle4.txt";
        public int FirstResult => 571;

        public long SecondResult => 917;

        public int Solve()
        {
            var points = 0;
            foreach(var line in allLines)
            {
                var assignment = line
                    .Split(',')
                    .SelectMany(x => x.Split('-'))
                    .Select(x => int.Parse(x))
                    .ToList();

                if(assignment[0] <= assignment[2] && assignment[1] >= assignment[3]
                    || assignment[0] >= assignment[2] && assignment[1] <= assignment[3]
                )
                {
                    points++;
                }
            }
            return points;
        }
        public long SolveNext()
        {
            var points = 0;
            foreach(var line in allLines)
            {
                var assignment = line
                    .Split(',')
                    .SelectMany(x => x.Split('-'))
                    .Select(x => int.Parse(x))
                    .ToList();

                if(assignment[0] >= assignment[2] && assignment[0] <= assignment[3]
                || assignment[1] >= assignment[2] && assignment[1] <= assignment[3]
                || assignment[2] >= assignment[0] && assignment[2] <= assignment[1]
                || assignment[3] >= assignment[0] && assignment[3] <= assignment[1]
                )
                {
                    points++;
                }
            }
            return points;
        }
    }
}