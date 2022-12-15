using System.IO;
using System.Collections.Generic;
using System.Linq;
using System;

namespace AdventOfCode2022
{
    public class Puzzle13 : IPuzzle
    {
        private IList<string> allLines;
        public Puzzle13()
        {
            this.allLines = File.ReadAllLines(path);
        }
        public static string path => "input/Puzzle13.txt";
        public int FirstResult => 5390;

        public long SecondResult => 19261;

        public int Solve()
        {
            var result = new List<int>();
            var totalPacketsInCorrectPosition = 0;
            foreach (var lines in allLines.SplitList(3))
            {
                totalPacketsInCorrectPosition++;
                if(Compare(lines[0],lines[1]))
                {
                    result.Add(totalPacketsInCorrectPosition);
                }
            }

            return result.Sum();
        }
        public static bool Compare(string leftPackage, string rightPackage)
        {
            IData left = ParseInput(leftPackage);
            IData right = ParseInput(rightPackage);

            while(true)
            {
                var compareStrategy = GetCompareStrategy((left.GetType(), right.GetType()));
                var compareResult = compareStrategy(left, right);
                if(compareResult.Item1)
                {
                    return true;
                }
                else if(compareResult.Item2)
                {
                    return false;
                }
                else
                {
                    IData nextLeft;
                    IData nextRight;
                    var any = TryDequeue(left, out nextLeft, right, out nextRight);
                    if(any.Item1 != any.Item2)
                    {
                        if(any.Item2)
                        {
                            return true;
                        }
                        return false;
                    } 
                    if(any.Item1 == false && any.Item2 == false)
                    {
                        // return true;
                        return false;
                    }
                    else 
                    {
                        left = nextLeft;
                        right = nextRight;
                    }
                }
            }
        }
        static (bool, bool) TryDequeue(IData left, out IData nextLeft, IData right, out IData nextRight)
        {
            var anyLeft = left.TryDequeue(out nextLeft);
            var anyRight = right.TryDequeue(out nextRight);

            if(anyLeft == false && anyRight == false)
            {
                if(left.Parent != null && right.Parent != null)
                {
                    return TryDequeue(left.Parent, out nextLeft, right.Parent, out nextRight);
                }
                else 
                {
                    anyLeft = left.Parent?.TryDequeue(out nextLeft) ?? false;
                    anyRight = right.Parent?.TryDequeue(out nextRight) ?? false;
                }
            }

            return (anyLeft, anyRight);
        }
        bool TryDequeueParentReqursive(IData data, out IData next)
        {
            if(data.Parent != null)
            {
                return data.Parent.TryDequeue(out next) ? true : this.TryDequeueParentReqursive(data.Parent, out next);
            }
            next = null;
            return false;
        }
        static (bool,bool) CompareNumber(Item left, Item right)
        {
            return (left.Number < right.Number, right.Number < left.Number);
        }
        static (bool,bool) CompareMixed(List list, Item item)
        {
            InsertFirstInQueue(list, item.Number);

            return (false, false);
        }
        static void InsertFirstInQueue(List list, int number)
        {
            var newQueue = new Queue<IData>();
            newQueue.Enqueue(list.CreateNumber(number));
            
            while(list.datas.TryDequeue(out var item))
            {
                newQueue.Enqueue(item);
            }
            list.datas = newQueue;
        }
        static Func<IData,IData,(bool, bool)> GetCompareStrategy((Type a, Type b) bb)
        {
            var stratigies = new Dictionary<(Type, Type), Func<IData,IData,(bool,bool)>>()
            {
                {(typeof(List), typeof(List)), (left,right) => (false,false)},
                {(typeof(Item), typeof(Item)), (left, right) => CompareNumber((Item)left, (Item)right)},
                
                {(typeof(List), typeof(Item)), (left, right) => CompareMixed(right.Parent, (Item)right)},
                {(typeof(Item), typeof(List)), (left, right) => CompareMixed(left.Parent, (Item)left)},
            };

            return stratigies[bb];
        }
        static bool TakeWhileCondition(char c) => c switch 
        {
            '[' => false,
            ',' => false,
            ']' => false,
            _ => true,
        };
        static List ParseInput(string input)
        {
            var current = new List();
            List last = current;
            for(int i = 0; i<input.Length; i++)
            {
                var newInput = new string(input
                    .Skip(i)
                    .TakeWhileIncludingSelf(TakeWhileCondition)
                    .ToArray());

                if(newInput.StartsWith('['))
                {
                    last = last.AddList();
                }
                else if(newInput.StartsWith(']'))
                {
                    last = last.Parent;
                }
                else if(int.TryParse(newInput, out int number))
                {
                    last.AddNumber((int)number);
                    i += newInput.Length-1;
                }
            }
            return current;
        }
        public interface IData
        {
            int Number{ get; }
            Queue<IData> datas{ get; }
            List Parent {get;}
            bool TryDequeue(out IData data);
        }
        public class Item : IData
        {
            public Item(int number)
            {
                this.Number = number;
            }            
            public List Parent {get;set;}
            public int Number{ get; }
            public Queue<IData> datas => throw new NotImplementedException();
            public bool TryDequeue(out IData data)
            {
                return this.Parent.TryDequeue(out data);
            }
        }
        public class List : IData
        {
            public int Number => throw new NotImplementedException();
            public int NumberOfDatas => this.datas.Count();
            public Queue<IData> datas { get; set;}
            public List Parent {get; private set;}
            public List()
            {
                this.datas = new Queue<IData>();
            }
            public List AddList()
            {
                var data = new List();
                data.Parent = this;
                this.datas.Enqueue(data);
                return data;
            }
            public Item CreateNumber(int a)
            {
                var item = new Item(a);
                item.Parent = this;
                return item;
            }
            public void AddNumber(int a)
            {
                this.datas.Enqueue(this.CreateNumber(a));
            }
            public bool TryDequeue(out IData data)
            {
                return this.datas.TryDequeue(out data);
            }
        }

        public long SolveNext()
        {
            var allLines = new List<string>();
            foreach(var line in this.allLines.SplitList(3))
            {
                allLines.Add(line[0]);
                allLines.Add(line[1]);
            }
            var first = "[[2]]";
            allLines.Add(first);
            var second = "[[6]]";
            allLines.Add(second);
            
            allLines.Sort(new IDataComparer());

            var indexes = allLines
                .Select((line, index) => new 
                    {
                        line = line,
                        Index = index + 1,
                    })
                .Where(p => p.line == first || p.line == second)
                .ToList();
            
            return indexes[0].Index * indexes[1].Index;
        }

        public class IDataComparer : IComparer<string>
        {
            public int Compare(string x, string y)
            {
                return Puzzle13.Compare(x,y) == true ? -1 : 1;
            }
        }
    }
}