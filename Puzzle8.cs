using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2022
{
    public class Puzzle8 : IPuzzle
    {
        private IList<string> allLines;
        public Puzzle8()
        {
            this.allLines = File.ReadAllLines(path);
        }
        public static string path => "input/Puzzle8.txt";
        public int FirstResult => 1787;
        public long SecondResult => 440640;

        public int Solve()
        {
            var result = 0;
            var rowCount = allLines.Count;

            for(int row = 0; row < rowCount; row++)
            {
                for(int column = 0; column < rowCount; column++)
                {
                    if(this.CheckIsSeen(row, column, rowCount))
                    {
                        result++;
                    }
                }
            }
            return result;
        }
        private bool CheckIsSeen(int row, int column, int rowCount)
        {
            var value = this.allLines[row][column];

            var left = this.allLines[row].Take(column).All(x => x < value);
            var right = this.allLines[row].Substring(column+1, rowCount - (column+1)).All(x => x < value);
            var down = this.allLines.Take(row).All(x => x[column] < value);
            var up = this.allLines.TakeLast(rowCount - (row+1)).All(x => x[column] < value);

            return (left || right || up || down);
        }
        public long SolveNext()
        { 
            var rowCount = allLines.Count;
            long maxScore = 0;

            for(int row = 1; row < rowCount; row++)
            {
                for(int column = 1; column < rowCount; column++)
                {
                    maxScore = Math.Max(this.GetScoreFromTree(row,column,rowCount), maxScore);
                }
            }
            return maxScore;
        }
        private long GetScoreFromTree(int row, int column, int rowCount)
        {
            var value = this.allLines[row][column];

            var right = this.allLines[row].Skip(column+1).TakeWhileInclusive(x => x < value).Count();
            var left = this.allLines[row].Reverse().Skip(rowCount - column).TakeWhileInclusive(x => x < value).Count();
            var down = this.allLines.Skip(row+1).TakeWhileInclusive(x => x[column] < value).Count();
            var up = this.allLines.Reverse().Skip(rowCount - row).TakeWhileInclusive(x => x[column] < value).Count();

            return right * left * up * down;
        }
    }
}