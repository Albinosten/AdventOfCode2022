using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2022
{
    public class Puzzle2 : IPuzzle
    {
        //X for Rock, Y for Paper, and Z for Scissors
        //1 for Rock, 2 for Paper, and 3 for Scissors
        //A for Rock, B for Paper, and C for Scissors.
        //0 if you lost, 3 if the round was a draw, and 6 if you won
        private IList<string> allLines;
        private Dictionary<char,int> shapePoints;
        private Dictionary<char,char> shapeMapping;
        public Puzzle2()
        {
            this.allLines = File.ReadAllLines(path);
            this.shapePoints = new Dictionary<char, int>
            {
                {'X', 1},//Rock
                {'Y', 2},//Paper
                {'Z', 3},//Scissors
            };
            this.shapeMapping = new Dictionary<char, char>
            {
                {'A','X'},
                {'B','Y'},
                {'C','Z'},
            };
        }
        public static string path => "input/Puzzle2.txt";
        public int FirstResult => 9759;

        public long SecondResult => 12429;

        public int Solve()
        {
            var totalPoints = 0;
            foreach(var line in allLines)
            {
                var opponent = this.shapeMapping[line[0]];
                var me = line[2];

                totalPoints += this.shapePoints[me];

                //Win
                if(me - 1 == opponent || me +2 == opponent )
                {
                    totalPoints += 6;
                }
                //draw
                else if (me == opponent)
                {
                    totalPoints += 3;
                }
            }
           return totalPoints;
        }

//X means you need to lose, Y means you need to end the round in a draw, and Z means you need to win.
        public long SolveNext()
        {
            var totalPoints = 0;
            foreach(var line in allLines)
            {
                var opponent = this.shapeMapping[line[0]];
                var neededResult = line[2];

                char playedShape;
                //Win
                if(neededResult == 'Z')
                {
                    totalPoints += 6;
                    playedShape = (char)(opponent + 1);
                    if(opponent == 'Z')
                    {
                        playedShape = 'X';
                    }
                }
                //draw
                else if (neededResult == 'Y')
                {
                    totalPoints += 3;
                    playedShape = opponent;
                }
                //loose
                else 
                {
                    playedShape = (char)(opponent - 1);
                    if(opponent == 'X')
                    {
                        playedShape = 'Z';
                    }
                }
                totalPoints+=this.shapePoints[playedShape];
            }
            return totalPoints;
        }
    }
}