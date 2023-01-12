using System;
using System.Diagnostics;

namespace AdventOfCode2022
{
    public enum Part
    {
        One,
        Two,
    }
    class Program
    {
        static void Main(string[] args)
        {
            var watch = new Stopwatch();
            watch.Start();

            // OutputResult(new Example());
            // OutputResult(new Puzzle1());
            // OutputResult(new Puzzle2());
            // OutputResult(new Puzzle3());
            // OutputResult(new Puzzle4());
            // OutputResult(new Puzzle5());
            // OutputResult(new Puzzle6());
            // OutputResult(new Puzzle7());
            // OutputResult(new Puzzle8());
            // OutputResult(new Puzzle9());
            // OutputResult(new Puzzle10());
            // OutputResult(new Puzzle11());
            // OutputResult(new Puzzle12());
            // OutputResult(new Puzzle13());
            // OutputResult(new Puzzle14());
            // OutputResult(new Puzzle15());
            // OutputResult(new Puzzle16());
            // OutputResult(new Puzzle17());
            // OutputResult(new Puzzle18());
            // OutputResult(new Puzzle19());
            OutputResult(new Puzzle20());
            
            watch.Stop();
            Console.WriteLine("Total time run: "+ watch.Elapsed);

        }
        private static void OutputResult<T,J>(IPuzzle<T,J> puzzle)
        {
            var watch = new Stopwatch();
            watch.Start();

            Console.WriteLine("Now solving: " + puzzle.GetType().Name);
            Console.WriteLine("First: " + puzzle.Solve() + " Should be: " + puzzle.FirstResult);
            Console.WriteLine("Second: " + puzzle.SolveNext() + " Should be: " + puzzle.SecondResult);
            
            watch.Stop();
            Console.WriteLine("Time for puzzle: "+ watch.Elapsed);
        }
    }
}
