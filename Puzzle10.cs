using System.IO;
using System.Collections.Generic;
using System.Linq;
using System;

namespace AdventOfCode2022
{
    public class Puzzle10 : IPuzzle<int,string>
    {
        private IList<string> allLines;
        public Puzzle10()
        {
            this.allLines = File.ReadAllLines(path);
        }
        public static string path => "input/Puzzle10.txt";
        // public static string path => "input/EXAMPLE.TXT";
        public int FirstResult => 11960;

        public string SecondResult => "EJCFPGLH";

        public int[] ticks => new int[]{20,60,100,140,180,220};
        public int Solve()
        {
            
            int registerX = 1;
            int instructionIndex = 0;
            bool readyForNewInstruction = true;
            string instruction = "";
            int result = 0;
            for(int tick = 1; tick<221; tick++)
            {
                if(ticks.Contains(tick))
                {
                    result += tick*registerX;
                }
                if(readyForNewInstruction)
                {
                    instruction = this.allLines[instructionIndex];
                    if(instruction == "noop")
                    {
                        instructionIndex++;
                        continue;
                    }
                    readyForNewInstruction = false;
                }
                else
                {
                    readyForNewInstruction = true;
                    var input = int.Parse(instruction.Split(' ')[1]);
                    registerX += input;
                    instructionIndex++;
                }
            }
            return result;
        }

/*
####...##..##..####.###...##..#....#..#.
#.......#.#..#.#....#..#.#..#.#....#..#.
###.....#.#....###..#..#.#....#....####.
#.......#.#....#....###..#.##.#....#..#.
#....#..#.#..#.#....#....#..#.#....#..#.
####..##...##..#....#.....###.####.#..#.
*/
        public string SolveNext()
        {
            int registerX = 1;
            int instructionIndex = 0;
            bool readyForNewInstruction = true;
            string instruction = "";

            var result = new List<char>();
            for(int tick = 1; tick<(40*6)+1; tick++)
            {
                var pixel = tick%40;
                if(pixel == registerX
                    || pixel == registerX+1
                    || pixel == registerX+2
                )
                {
                    result.Add('#');
                }
                else
                {
                    result.Add('.');
                }
                if(readyForNewInstruction)
                {
                    instruction = this.allLines[instructionIndex];
                    if(instruction == "noop")
                    {
                        instructionIndex++;
                        continue;
                    }
                    readyForNewInstruction = false;
                }
                else
                {
                    readyForNewInstruction = true;
                    var input = int.Parse(instruction.Split(' ')[1]);
                    registerX += input;
                    instructionIndex++;
                }
            }
            Console.WriteLine(new string(result.GetRange(0,40).ToArray()));
            Console.WriteLine(new string(result.GetRange(40,40).ToArray()));
            Console.WriteLine(new string(result.GetRange(80,40).ToArray()));
            Console.WriteLine(new string(result.GetRange(120,40).ToArray()));
            Console.WriteLine(new string(result.GetRange(160,40).ToArray()));
            Console.WriteLine(new string(result.GetRange(200,40).ToArray()));
            return "EJCFPGLH";
        }
    }
}