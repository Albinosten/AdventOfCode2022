using System.IO;
using System.Collections.Generic;
using System.Linq;
using System;

namespace AdventOfCode2022
{
    public class Puzzle6 : IPuzzle
    {
        private IList<string> allLines;
        public Puzzle6()
        {
            this.allLines = File.ReadAllLines(path);
        }
        public static string path => "input/Puzzle6.txt";
        public int FirstResult => 1198;

        public long SecondResult => 3120;

        public int Solve()
        {
			for (int i = 0; i < this.allLines[0].Length - 3; i++)
			{
				var a = this.allLines[0].Substring(i, 4);
				if (a.Distinct().Count() == 4)
				{
					return i + 4;
				}
			}
			return 0;
        }
        public long SolveNext()
        {for (int i = 0; i < this.allLines[0].Length - 13; i++)
			{
				var a = this.allLines[0].Substring(i, 14);
				if (a.Distinct().Count() == 14)
				{
					return i + 14;
				}
			}
			return 0;
        }
    }
}