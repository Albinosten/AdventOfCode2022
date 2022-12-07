using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2022
{
    public class Puzzle5 : IPuzzle<string>
    {
        private IList<string> allLines;
        public Puzzle5()
        {
            this.allLines = File.ReadAllLines(path);
        }
        public static string path => "input/Puzzle5.txt";
        public string FirstResult => "NTWZZWHFV";
        public string SecondResult => "BRZGFVBTJ";

        public string Solve()
        {
            return this.Solve(true);
        }
        public string SolveNext()
        {
            return this.Solve(false);
        }        
        private string Solve(bool keepOrder)
        {
            var crates = new List<Stack<char>>(8);
            for(int i = 0; i<9; i++)
            {
                crates.Add(new Stack<char>());
            }

            for(int i = 7; i >= 0; i--)
            {
                var input = this.allLines[i];
                this.AddValue(crates, 0, input[1]);
                this.AddValue(crates, 1, input[5]);
                this.AddValue(crates, 2, input[9]);
                this.AddValue(crates, 3, input[13]);
                this.AddValue(crates, 4, input[17]);
                this.AddValue(crates, 5, input[21]);
                this.AddValue(crates, 6, input[25]);
                this.AddValue(crates, 7, input[29]);
                this.AddValue(crates, 8, input[33]);
            }

            foreach(var move in this.allLines.Skip(10))
            {
                var input = move.Split(" ");
                var moveFrom = int.Parse(input[3]);
                var moveTo  = int.Parse(input[5]);

                var containers = new List<char>();
                for(int i = 0; i < int.Parse(input[1]); i++)
                {
                    containers.Add(crates[moveFrom-1].Pop());
                }
                foreach(var container in keepOrder ? containers : containers.Reverse<char>())
                {
                    crates[moveTo-1].Push(container);
                }
            }

            return new string(crates.Select(x => x.Pop()).ToArray());
        }
        private void AddValue(List<Stack<char>> crates, int index, char value)
        {
            if(char.IsLetter(value))
            {
                crates[index].Push(value);
            }
        }
    }
}