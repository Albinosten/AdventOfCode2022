using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2022
{
    public class Puzzle7 : IPuzzle<string>
    {
        private IList<string> allLines;
        public Puzzle7()
        {
            this.allLines = File.ReadAllLines(path);
        }
        public static string path => "input/EXAMPLE.TXT";
        // public static string path => "input/Puzzle7.txt";
        public string FirstResult => "";
        public string SecondResult => "";

        public string Solve()
        {
            
            return "";

        }
        public string SolveNext()
        {
             return"";
        }
       
    }
}