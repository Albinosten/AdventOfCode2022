using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2022
{
    public class Puzzle12 : IPuzzle
    {
        private IList<string> allLines;
        public Puzzle12()
        {
            this.allLines = File.ReadAllLines(path);
        }
        public static string path => "input/Puzzle12.txt";
        // public static string path => "input/EXAMPLE.TXT";
        public int FirstResult => 361;
        public long SecondResult => 354;
        
        int GetHeightFromChar(char c) => c switch
        {
            'S' => 'a',
            'E' =>  'z',
            var n => n,
        };
        int GetValue((int x,int y) pos)
        {
            var a = this.allLines[pos.y][pos.x];
            var b = this.GetHeightFromChar(a);
            return b;
        }
        (int, int)[] GetAdjacent((int x, int y) cord)
        {
            return new []
            {
                (cord.x+1, cord.y),
                (cord.x-1, cord.y),
                (cord.x, cord.y+1),
                (cord.x, cord.y-1),
            };
        }

        bool Filter((int x, int y) next, (int x, int y) current)
        {
            var firstValue = GetValue(next);
            var secondValue = GetValue(current);
            if( firstValue - secondValue <= 1)
            {
                return true;
            }
            return false;   
        }
        bool FilterPt2((int x, int y) next, (int x, int y) current)
        {
            var firstValue = GetValue(next);
            var secondValue = GetValue(current);
            if( firstValue - secondValue >= -1)
            {
                return true;
            }
            return false;   
        }
        IEnumerable<(int, int)> FilterOutOfBound((int x, int y)[] cords, (int x, int y) max)
        {
            for(int i = 0; i < cords.Count(); i++)
            {
                var cord = cords[i];
                if(cord.x <= max.x && cord.y <= max.y && cord.x >= 0 && cord.y >= 0
                )
                {
                    yield return cord;
                }
            }
        }
        

        public int Solve()
        {
            var start = (0,0);
            var end = (0,0);
            for(var y = 0; y < allLines.Count; y++)
            {
                var row = allLines[y];
                for(var x = 0; x < row.Count(); x++)
                {
                    if(row[x] == 'S')
                    {
                        start = (x,y);
                    }
                    if(row[x] == 'E')
                    {
                        end = (x,y);
                    }
                }
            }
            //362 too high;
            return this.Solver(start,end, Filter);
        }

        private int Solver((int, int) start, (int, int) end, Func<(int,int), (int,int), bool> filter, Part part = Part.One)
        {
            
            var max = (allLines[0].Count()-1, allLines.Count-1);
            var visited = new HashSet<(int,int)>();

            var next = new HashSet<(int,int)>();
            next.Add(start);
            var stepps = 0;
            visited.Add(start);
            while(next.Count > 0)
            {
                var nextBatch = new HashSet<(int,int)>();
                foreach (var current in next)
                {
                    if( part == Part.One)
                    {
                        if(current == end)
                        {
                            return stepps;
                        }
                    }
                    else
                    {
                        if(GetValue(current) == 'a')
                        {
                            return stepps;
                        }
                    }
                    
                    visited.Add(current);
                    foreach (var item in FilterOutOfBound(GetAdjacent(current), max)
                        .Where(x => filter(x, current))
                        .Where(x => !visited.Contains(x)))
                    {
                        nextBatch.Add(item);
                    }

                }
                next = nextBatch;
                stepps++;
            }
            return stepps;
        }
        public long SolveNext()
        {
            var start = (0,0);
            var end = (0,0);
            for(var y = 0; y < allLines.Count; y++)
            {
                var row = allLines[y];
                for(var x = 0; x < row.Count(); x++)
                {
                    if(row[x] == 'S')
                    {
                        start = (x,y);
                    }
                    if(row[x] == 'E')
                    {
                        end = (x,y);
                    }
                }
            }

            return this.Solver(end, start, FilterPt2, Part.Two);
        }
        
    }
}