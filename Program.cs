using System;

using System.Diagnostics;
using System.Threading;

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

            OutputResult(new Puzzle1());
            OutputResult(new Puzzle2());
            OutputResult(new Puzzle3());
            OutputResult(new Puzzle4());
            OutputResult(new Puzzle5());
            OutputResult(new Puzzle6());
            OutputResult(new Puzzle7());
            OutputResult(new Puzzle8());
            OutputResult(new Puzzle9());
            OutputResult(new Puzzle10());
            OutputResult(new Puzzle11());
            OutputResult(new Puzzle12());
            OutputResult(new Puzzle13());

            watch.Stop();
            Console.WriteLine("Total time run: "+ watch.Elapsed);

            // OutputResult(new Example());
        }
        private static void OutputResult<T,J>(IPuzzle<T,J> puzzle)
        {
            Console.WriteLine("Now solving: " + puzzle.GetType().Name);
            Console.WriteLine("First: " + puzzle.Solve() + " Should be: " + puzzle.FirstResult);
            Console.WriteLine("Second: " + puzzle.SolveNext() + " Should be: " + puzzle.SecondResult);
        }
    }
}
