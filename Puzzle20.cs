using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2022
{
	public class Puzzle20 : IPuzzle
	{
        public int FirstResult => 15297;
        public long SecondResult => 2897373276210;
        // public static string source => "input/EXAMPLE.TXT";
        public static string source => "input/Puzzle20.txt";

        public int Solve()
        {
			var list = new List<(int originIndex, long value)>();

			var lines = File.ReadLines(source).ToList();
			for (int i = 0; i < lines.Count; i++)
			{
				list.Add((i, int.Parse(lines[i])));
			}

			for (int i = 0; i < lines.Count; i++)
			{
				this.Move(list, i);
			}

			var zeroIndex = list.IndexOf(list.First(x => x.value == 0));
			var a = list[this.GetIndex(list.Count, zeroIndex + 1000)].value;
			var b = list[this.GetIndex(list.Count, zeroIndex + 2000)].value;
			var c = list[this.GetIndex(list.Count, zeroIndex + 3000)].value;

            return (int)(a+b+c);
        }
		public long SolveNext() 
		{
            long key = 811589153;
            var list = new List<(int originIndex, long value)>();

			var lines = File.ReadLines(source).ToList();
			for (int i = 0; i < lines.Count; i++)
			{
				list.Add((i, int.Parse(lines[i])*key));
			}

            for(int times = 0; times<10;times++){
                for (int i = 0; i < lines.Count; i++)
                {
                    this.Move(list, i);
                }
			}

			var zeroIndex = list.IndexOf(list.First(x => x.value == 0));

            return new[]{1000,2000,3000}
                .Sum(x => list[this.GetIndex(list.Count, zeroIndex + x)].value);
		}
		private void Move(List<(int originIndex, long value)> list, int i)
		{
            if(i == 34){

            }
			var index = list.IndexOf(list.First(x => x.originIndex == i));
			var value = list[index].value;
			list.RemoveAt(index);

			var newIndex = this.GetIndex(list.Count, index+value);
			if(newIndex == 0)
			{
				list.Add((i,value));
			}
			else
			{
				list.Insert(newIndex, (i, value));
			}
		}
		int GetIndex(long listCount, long index)
		{
            var value = Math.Max(Math.Abs(index) / (listCount - 1), 1) +1;

			return (int)((listCount * value + index) % (listCount));
		}
	}
}
