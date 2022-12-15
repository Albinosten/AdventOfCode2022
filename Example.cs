using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2022
{
    public class Example : IPuzzle
    {
        private IList<string> allLines;
        public Example()
        {
            this.allLines = File.ReadAllLines(path);
        }
        public static string path => "input/EXAMPLE.TXT";
        // public static string path => "input/Puzzle13.txt";
        public int FirstResult => 0;
        public long SecondResult => 0;

        public int Solve()
        {
            return 0;
        }
        public long SolveNext()
        { 
            return 0;
        }
    }
}