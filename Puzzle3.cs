using System.IO;
using System.Collections.Generic;
using System.Linq;
using System;

namespace AdventOfCode2022
{
    public class Puzzle3 : IPuzzle
    {
        private IList<string> allLines;
        public Puzzle3()
        {
            this.allLines = File.ReadAllLines(path);
        }
        public static string path => "input/Puzzle3.txt";
        public int FirstResult => 8153;

        public long SecondResult => 2342;
//Lowercase item types a through z have priorities 1 through 26.
//Uppercase item types A through Z have priorities 27 through 52.

        public int Solve()
        {
            var points = 0;
            foreach(var line in allLines)
            { 
                var compartmentOne = line.Take(line.Length / 2);
                var compartmentTwo = line.TakeLast(line.Length / 2);

                var item = compartmentOne.Where(x => compartmentTwo.Contains(x)).First();
                points += this.GetPoint(item);
            }
            return points;
        }

        public long SolveNext()
        {
            var points = 0;
            foreach(var badgeGroup in allLines.SplitList(3))
            {
                var badge = badgeGroup[0]
                    .Where(x => badgeGroup[1].Contains(x))
                    .Where(x => badgeGroup[2].Contains(x))
                    .First();
                points += this.GetPoint(badge);
            }
            return points;
        }
        private int GetPoint(Char a)
        {
            var offset = a < 97 ? 38 : 96;
            return  a - offset;
        }
        
    }
}