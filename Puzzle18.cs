using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace AdventOfCode2022
{
	public class Puzzle18 : IPuzzle
	{

		public static string source => "input/Puzzle18.txt";
        public int FirstResult => 4340;
        public long SecondResult => 0;
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

						&& (cube.x < cube2.x || cube.y < cube2.y || cube.z < cube2.z)
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
            return 0;
        }

	}

}
