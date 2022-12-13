using System.IO;
using System.Collections.Generic;
using System.Linq;
using System;

namespace AdventOfCode2022
{
    public class Puzzle11 : IPuzzle<long>
    {
        private IList<string> allLines;
        public Puzzle11()
        {
            this.allLines = File.ReadAllLines(path);
        }
        public static string path => "input/Puzzle11.txt";
        public long FirstResult => 113232;

        public long SecondResult => 29703395016;

        public long Solve()
        {
            return this.Solve(20, Part.One);
        }
        public long SolveNext()
        {
            return this.Solve(10000, Part.Two);
        }
        private long Solve(int iterations, Part part)
        {
            var monkeys = this.CreateMonkeys();

            for(int i = 0; i < iterations; i++)
            {
                foreach (var monkey in monkeys)
                {
                    while(monkey.Process(monkeys, part));
                }
            }

            return monkeys
                .OrderByDescending(x => x.Inspected)
                .Select(x => x.Inspected)
                .Take(2)
                .Multiply();
        }
        private IList<Monkey> CreateMonkeys()
        {
            var monkeys = new List<Monkey>();
            foreach(var monkeyInput in this.allLines.SplitList(7))
            {
                monkeys.Add(new Monkey(monkeyInput));
            }
            return monkeys;
        }
        private class Monkey
        {
            public Queue<long> items;
            private string operation;
            public long test;
            private int trueIndex;
            private int falseIndex;
            private string monkeyName;
            public Monkey(IList<string> input)
            {
                this.monkeyName = input[0].Split(' ')[1];
                this.items = new Queue<long>();
                foreach (var item in input[1].Substring(18).Split(", "))
                {    
                    this.items.Enqueue(int.Parse(item));
                }
                this.operation = input[2].Substring(23);
                
                this.test = int.Parse(input[3].Split(' ').Last());
                this.trueIndex = int.Parse(input[4].Split(' ').Last());
                this.falseIndex = int.Parse(input[5].Split(' ').Last());
            }
            public long Inspected = 0;
            public bool Process(IList<Monkey> allMonkeys, Part part)
            {
                if(this.items.TryDequeue(out long worry))
                {
                    this.Inspected++;
                    var oper = this.operation.Split(' ')[0][0];
                    var value = this.operation.Split(' ')[1];

                    worry = this.getNewWorry(oper, value, worry);
                    if(part == Part.One)
                    {
                        worry = worry/3;
                    }
                    else 
                    {
                        worry = worry % allMonkeys
                            .Select(x => x.test)
                            .Multiply();
                    }

                    var index = worry % this.test == 0 ? this.trueIndex : this.falseIndex;
                    allMonkeys[index].AddItem(worry);
                    
                    return true;
                }
                return false;
            }

            long GetWorry(string value, long worry) =>  value switch 
            {
                "old" => worry,
                var a => int.Parse(a),
            };
            long getNewWorry(char oper, string value, long worry) =>  oper switch 
            {
                '*' => GetWorry(value, worry) * worry,
                '+' => GetWorry(value, worry) + worry,
                _ => throw new NotImplementedException(),
            };
            public void AddItem(long worth)
            {
                this.items.Enqueue(worth);
            }
        }
    }
}