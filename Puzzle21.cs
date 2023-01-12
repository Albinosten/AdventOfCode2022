using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2022
{
	public class Puzzle21 : IPuzzle<long>
	{
        public long FirstResult => 324122188240430;
        public long SecondResult => 0;
        // public static string source => "input/EXAMPLE.TXT";
        public static string source => "input/Puzzle21.txt";

        public long Solve()
        {
            var monkeys = this.CreateMonkeys();

            while(!monkeys["root"].CanYell)
            {
                foreach(var monkey in monkeys.Values.Where(x => !x.CanYell))
                {
                    if(monkeys[monkey.FirstNext].CanYell && monkeys[monkey.SecondNext].CanYell)
                    {
                        this.TeachMonkeyItsValue(monkeys[monkey.FirstNext].Value, monkeys[monkey.SecondNext].Value, monkey);
                    }
                }
            }

            return monkeys["root"].Value;
        }
		public long SolveNext() 
		{

            var monkeys = this.CreateMonkeys();

            while(!monkeys[monkeys["root"].SecondNext].CanYell)
            {
                foreach(var monkey in monkeys.Values.Where(x => !x.CanYell))
                {
                    if(monkeys[monkey.FirstNext].CanYell && monkeys[monkey.SecondNext].CanYell)
                    {
                        this.TeachMonkeyItsValue(monkeys[monkey.FirstNext].Value, monkeys[monkey.SecondNext].Value, monkey);
                    }
                }
            }

            return 0;
		}

        private void ResetMonkeys(Dictionary<string,Monkey> monkeys)
        {
            foreach(var monkey in monkeys.Values.Where(x => !string.IsNullOrEmpty(x.FirstNext)))
            {
                monkey.CanYell = false;
            }
        }
        private void TeachMonkeyItsValue(long firstValue, long secondValue, Monkey monkey)
        {
            monkey.CanYell = true;
            monkey.Value = this.GetValue(firstValue,secondValue,monkey.Operator);
        }

        private long GetValue(long firstValue, long secondValue, char @operator) =>  @operator switch
        {

            '+' => firstValue + secondValue,
            '-' => firstValue - secondValue,
            '*' => firstValue * secondValue,
            '/' => firstValue / secondValue,
            _ => throw new InvalidOperationException(),
        };
        private Dictionary<string, Monkey> CreateMonkeys()
        {
            var result = new Dictionary<string,Monkey>();
            foreach(var line in File.ReadLines(source))
            {
                var splittedLine = line
                    .Split(new []{':',' '})
                    .ToList();
                var monkey = new Monkey();
                if(int.TryParse(splittedLine[2], out int value))
                {
                    monkey.CanYell = true;
                    monkey.Value = value;
                }
                else
                {
                    monkey.FirstNext = splittedLine[2];
                    monkey.Operator = splittedLine[3][0];
                    monkey.SecondNext = splittedLine[4];
                }
                result.Add(splittedLine[0],monkey);
            }
            return result;
        }
        private class Monkey
        {
            public bool CanYell;
            public char Operator;
            public string FirstNext;
            public string SecondNext;
            public long Value;
        }
	}
}
