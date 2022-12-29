using System.Collections.Generic;
using System.Linq;
using System;
using System.IO;

namespace AdventOfCode2022
{
	
	public class Puzzle17 : IPuzzle<long>
	{
        public long Solve()
        {
            return this.Solve(2022);
        }
        public long SolveNext()
        {
            return this.Solve(1000000000000);
        }

        public long FirstResult => 3159;
        public long SecondResult => 1566272189352; 
		private string source => @"input/Puzzle17.txt";
		int numberOfDistinctRocks => 5;
		public long Solve(long rocksToSimulate)
		{
			var input = File.ReadAllText(source);

			var filledPositions = new HashSet<(int x, long y)>();
			_ = this.GetRock(-1).Select(x => filledPositions.Add(x)).ToList();

			int moves = 0;
			int startRepeatingI = 0;
			int  startRepeatingY = 0;
			long yMax = 0;
			long extraY = 0;

			for (long i = 0; i < rocksToSimulate; i++)
			{
				var rock = this.GetRock(i % 5)
					.SetRockStartPosition(yMax);
				while (rock.CanMove(filledPositions))
				{
					if (moves == input.Length * numberOfDistinctRocks)
					{
						startRepeatingI = (int)i;
						startRepeatingY = (int)yMax;
					}
					else if (moves == input.Length * numberOfDistinctRocks * 2)
					{
						var diffI = i - startRepeatingI;
						var diffY = yMax - startRepeatingY;

						var numberOfSkipps = (rocksToSimulate - i) / diffI;
						rocksToSimulate -= (diffI * numberOfSkipps);
						extraY = (diffY * numberOfSkipps);
					}

					rock.ApplyMove(filledPositions, GetMove(input[moves % input.Length]));
					rock.MoveDown();
					moves++;
				}
				rock.MoveUp();
				yMax = Math.Max(rock.Max(r => r.y), yMax);

				_ = rock.Select(x => filledPositions.Add(x)).ToList();
			}
			return yMax + extraY;
		}
		private static int GetMove(char move) => move switch
		{
			'<' => -1,
			'>' => 1,
			_ => throw new InvalidOperationException(),
		};

		(int x, long y)[] GetRock(long number) => number switch
		{
			-1 => new[] {(0, 0L), (1, 0), (2, 0), (3, 0), (4, 0), (5, 0), (6, 0) }, //floor
			0 => new[] { (0, 0L), (1, 0), (2, 0), (3, 0) },
			1 => new[] { (1, 0L), (0, 1), (1, 1), (2, 1), (1, 2) },
			2 => new[] { (0, 0L), (1, 0), (2, 0), (2, 1), (2, 2) },
			3 => new[] { (0, 0L), (0, 1), (0, 2), (0, 3) },
			4 => new[] { (0, 0L), (1, 0), (0, 1), (1, 1) },

			_ => null,
		};
		
	}
    public static class Rock
	{
		public static (int x, long y)[] SetRockStartPosition(this (int x, long y)[] rock, long yMax)
		{
			return rock.Select(pos => (pos.x + 2, pos.y + yMax + 4)).ToArray();
		}
		public static bool CanMove(this (int x, long y) pos, HashSet<(int x, long y)> filledPositions)
		{
			return !filledPositions.Contains(pos) && pos.x >=0 && pos.x < 7;
		}
		public static bool CanMove(this (int x, long y)[] rock, HashSet<(int x, long y)> filledPositions)
		{
			return rock.All(x => x.CanMove(filledPositions));
		}
		public static void ApplyMove(this (int x, long y)[] rock, HashSet<(int x, long y)> filledPositions, int move)
		{
			var canMove = rock.All(r => (r.x + move, r.y).CanMove(filledPositions));
			if (!canMove) return;

			for(int i= 0; i < rock.Length; i++)
			{
				rock[i].x += move;
			}
		}
		public static void MoveDown(this (int x, long y)[] rock)
		{
			rock.MoveY(-1);
		}
		public static void MoveUp(this (int x, long y)[] rock)
		{
			rock.MoveY(1);
		}
		public static void MoveY(this (int x, long y)[] rock, long y) 
		{
			for (int i = 0; i < rock.Length; i++)
			{
				rock[i].y+=y;
			}
		}
	}
}