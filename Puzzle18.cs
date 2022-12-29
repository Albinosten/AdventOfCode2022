using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
namespace AdventOfCode2022
{
	public class Puzzle18 : IPuzzle
	{
		public static string source => "input/Puzzle18.txt";    
        public int FirstResult => 4340;
        public long SecondResult => 2468;
		public int Solve() 
		{
			var allLines = File.ReadAllLines(source);
			var intersections = 0;
			for(int i = 0; i < allLines.Length; i++)
			{
				var temp = allLines[i].Split(',');
				(int x, int y, int z) cube = (int.Parse(temp[0]), int.Parse(temp[1]), int.Parse(temp[2]));

				for (int u = i+1; u < allLines.Length; u++)
				{
					temp = allLines[u].Split(',');
					(int x, int y, int z) cube2 = (int.Parse(temp[0]), int.Parse(temp[1]), int.Parse(temp[2]));

					if(((cube.x == cube2.x && cube.y == cube2.y && Math.Abs(cube.z - cube2.z) == 1)
						|| (cube.x == cube2.x && cube.z == cube2.z && Math.Abs(cube.y - cube2.y) == 1)
						|| (cube.y == cube2.y && cube.z == cube2.z && Math.Abs(cube.x - cube2.x) == 1))
						) 
					{
						intersections++;
					}
				}
			}
			return (allLines.Length * 6) - (intersections * 2);
		}
        public long SolveNext()
        {
            var allLines = File.ReadAllLines(source);
            var cubes = new List<(int x, int y, int z)>();
			for(int i = 0; i < allLines.Length; i++)
			{
				var temp = allLines[i].Split(',');
				 cubes.Add((int.Parse(temp[0]), int.Parse(temp[1]), int.Parse(temp[2])));
			}

            var result = 0;
            var visited = new HashSet<(int x, int y, int z)>();
            var q = new Queue<(int x, int y, int z)>();
            q.Enqueue((-1,-1,-1));

            while(q.Any())
            {
                var n = q.Dequeue();
                if(n.IsInside() && !cubes.Contains(n) && !visited.Contains(n))
                {
                    visited.Add(n);
                    foreach(var neighbour in n.Neighbours().Where(x => x.IsInside()))
                    {
                        q.Enqueue(neighbour);
                    }
                }
                else if(cubes.Contains(n))
                {
                    result++;
                }
            }

			return result;
        }
	}

    public static class water
    {
        static int min = -1;
        static int max = 22;

        public static (int x, int y, int z)[] Neighbours(this (int x, int y, int z) pos)
        {
            return new []
            {
                (pos.x+1, pos.y, pos.z),
                (pos.x-1, pos.y, pos.z),
                (pos.x, pos.y+1, pos.z),
                (pos.x, pos.y-1, pos.z),
                (pos.x, pos.y, pos.z+1),
                (pos.x, pos.y, pos.z-1),
            };
        }
        public static bool IsInside(this (int x, int y, int z) pos)
        {
            return pos.x.IsInside() && pos.y.IsInside() && pos.z.IsInside();
        }
        static bool IsInside(this int value)
        {
            return min <= value && value <= max;
        }
    }
}
