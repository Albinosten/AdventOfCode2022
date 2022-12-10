using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2022
{
    public class Puzzle9 : IPuzzle
    {
        private IList<string> allLines;
        public Puzzle9()
        {
            this.allLines = File.ReadAllLines(path);
        }
        public static string path => "input/Puzzle9.txt";
        public int FirstResult => 6175;
        public long SecondResult => 2578;
        
        public int Solve()
        {
            return this.Solve(2);
        }
        public long SolveNext()
        {
            return this.Solve(10);
        }

        private bool ShouldChange( Knot head,Knot tail)
        {
            return Math.Max(Math.Abs(head.x - tail.x), Math.Abs(head.y - tail.y)) > Math.Sqrt(2);
        }
        public class Knot
        {
            public Knot(int x, int y)
            {
                this.x = x;
                this.y = y;
            }            
            public int x {get;set;}
            public int y {get;set;}
        }
        private int Solve(int knots)
        {

            var visitedPositionsByTail = new HashSet<(int,int)>();
            var tails = new List<Knot>(knots);
            for(int i = 0;i<tails.Capacity;i++)
            {
                tails.Add(new Knot(0,0));
            }

            foreach(var input in this.allLines)
            {
                var direction = input.Split(' ')[0][0];
                var count = int.Parse(input.Split(' ')[1]);
                for(int moves = 0; moves<count; moves++)
                {
                    Knot change;
                    if(direction == 'R')
                    {
                        change = new (1,0);
                    }
                    else if(direction == 'L')
                    {
                        change = new (-1, 0);
                    }
                    else if(direction == 'U')
                    {
                        change = new (0, 1);
                    }
                    else // direction == 'D'
                    {
                        change = new (0, -1);
                    }

                    tails[0] = this.AddTogether(tails[0], change);
                    for(int i = 1; i < tails.Capacity; i++)
                    {

                        if(this.ShouldChange(tails[i], tails[i-1]))
                        {
                            var head = tails[i-1];
                            var tail = tails[i];
                            if(head.x > tail.x) tail.x++;
                            if(head.x < tail.x) tail.x--;
                            if(head.y > tail.y) tail.y++;
                            if(head.y < tail.y) tail.y--;
                        }
                        var last = tails[tails.Capacity-1];
                        visitedPositionsByTail.Add(new (last.x,last.y));
                    }   
                }
            }
            return visitedPositionsByTail.Count();
        }
         
        private Knot AddTogether(Knot first,Knot second)
        {
            return new (first.x + second.x, first.y + second.y);
        }
    }
}