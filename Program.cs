using System;

namespace AdventOfCode2022
{
    class Program
    {
        static void Main(string[] args)
        {
            OutputResult(new Puzzle1());
            OutputResult(new Puzzle2());
            OutputResult(new Puzzle3());
            OutputResult(new Puzzle4());
            OutputResult(new Puzzle5());
            OutputResult(new Puzzle6());
            OutputResult(new Puzzle7());
        }

        private static void OutputResult(IPuzzle puzzle)
        {
            Console.WriteLine("Now solving: " + puzzle.GetType().Name);
            Console.WriteLine("First: " + puzzle.Solve() + " Should be: " + puzzle.FirstResult);
            Console.WriteLine("Second: " + puzzle.SolveNext() + " Should be: " + puzzle.SecondResult);
        }
        private static void OutputResult(IPuzzle<string> puzzle)
        {
            Console.WriteLine("Now solving: " + puzzle.GetType().Name);
            Console.WriteLine("First: " + puzzle.Solve() + " Should be: " + puzzle.FirstResult);
            Console.WriteLine("Second: " + puzzle.SolveNext() + " Should be: " + puzzle.SecondResult);
        }
        private static void OutputResult(IPuzzle<long> puzzle)
        {
            Console.WriteLine("Now solving: " + puzzle.GetType().Name);
            Console.WriteLine("First: " + puzzle.Solve() + " Should be: " + puzzle.FirstResult);
            Console.WriteLine("Second: " + puzzle.SolveNext() + " Should be: " + puzzle.SecondResult);
        }
    }
}
