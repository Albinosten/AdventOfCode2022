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
        public static string path => "input/example.txt";
        public int FirstResult => 2;

        public long SecondResult => 3;

        public int Solve()
        {
            foreach(var line in allLines)
            {

            }
            return this.allLines.Count();

        }

        public long SolveNext()
        {
            
return 3;
        }
    }
}