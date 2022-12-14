using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
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
        // public static string path => "input/EXAMPLE.TXT";
        public int FirstResult => 0;

        public long SecondResult => 0;

        public int Solve()
        {
            var result = new List<int>();
            var totalPacketsInRightPosition = 0;
            foreach (var lines in allLines.SplitList(3))
            {
                totalPacketsInRightPosition++;

                // lines[0] = "[[8,5],[[[4,5],[6,7,1],[2,3,1,2,6],[6,7]],9],[7,5]]";
                // lines[1] = "[[],[],[3],[[],[[8,9,6],6,9,[10,9,6]],[6],9]]";
                var packetOne = this.ParseInput(lines[0], new List());
                var packetTwo = this.ParseInput(lines[1], new List());

                var keepOnGoing = true;
                IData left = packetOne;
                IData right = packetTwo;
                int totalCount = 0;
                while(keepOnGoing)
                {
                    totalCount++;
                    var compareStrategy = this.GetCompareStrategy((left.GetType(), right.GetType()));
                    var compareResult = compareStrategy(left, right);
                    if(compareResult.Item1)
                    {
                        //left is smaller. correct order
                        keepOnGoing = false;
                        result.Add(totalPacketsInRightPosition);
                    }

                    //have to get again because mixedStategy is type order dependent
                    //this.GetCompareStrategy((right.GetType(), left.GetType()))(right, left)
                    else if(compareResult.Item2)
                    {
                        //right is smaller. wrong order
                        keepOnGoing = false;
                    }
                    else
                    {
                        IData nextLeft;
                        IData nextRight;
                        var any = this.TryDequeue(left, out nextLeft, right, out nextRight);
                        
                        if(any.Item1 != any.Item2)
                        {
                            keepOnGoing = false;
                            if(any.Item2)
                            {
                                result.Add(totalPacketsInRightPosition);
                            }
                        }
                        if(any.Item1 == false && any.Item2 == false)
                        {
                            // a = false;
                            Console.WriteLine(lines[0]);
                            Console.WriteLine(lines[1]);
                            throw new NotImplementedException("hamnar jag här har jag gjort fel");
                            //borde inte komma hit eftersom att då är båda slut..
                        }
                        else 
                        {
                            left = nextLeft;
                            right = nextRight;
                        }
                    }
                }
            }
            foreach (var item in result)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine("total rows:"+result.Count());

            //5403 too high
            //4795 too low
            return result.Sum();
        }
        (bool, bool) TryDequeue(IData left, out IData nextLeft, IData right, out IData nextRight)
        {
            var anyLeft = left.TryDequeue(out nextLeft);
            var anyRight = right.TryDequeue(out nextRight);

            if(anyLeft == false && anyRight == false)
            {
                if(left.Parent != null && right.Parent != null)
                {
                    return this.TryDequeue(left.Parent, out nextLeft, right.Parent, out nextRight);
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
        (bool,bool) CompareNumber(Item left, Item right)
        {
            //left is smaller and in correct order
            return (left.Number < right.Number, right.Number < left.Number);
        }
        (bool, bool) CompareList(List left, List right)
        {
            return (false, false);
        }
        (bool,bool) CompareMixed(List list, Item item)
        {
            this.InsertFirstInQueue(list, item.Number);

            return (false, false);
        }
        void InsertFirstInQueue(List list, int number)
        {
            var newQueue = new Queue<IData>();
            newQueue.Enqueue(list.CreateNumber(number));
            
            while(list.datas.TryDequeue(out var item))
            {
                newQueue.Enqueue(item);
            }
            list.datas = newQueue;
        }
        Func<IData,IData,(bool, bool)> GetCompareStrategy((Type a, Type b) bb)
        {
            var stratigies = new Dictionary<(Type, Type), Func<IData,IData,(bool,bool)>>()
            {
                {(typeof(List), typeof(List)), (left,right) => this.CompareList((List)left, (List)right)},
                {(typeof(Item), typeof(Item)), (left, right) => this.CompareNumber((Item)left, (Item)right)},
                
                {(typeof(List), typeof(Item)), (left, right) => this.CompareMixed(right.Parent, (Item)right)},
                {(typeof(Item), typeof(List)), (left, right) => this.CompareMixed(left.Parent, (Item)left)},
            };

            return stratigies[bb];
        }
        bool TakeWhileCondition(char c) => c switch 
        {
            '[' => false,
            ',' => false,
            ']' => false,
            _ => true,
        };
         List ParseInput(string input, List current)
        {
            // input = "[1,[2,[3,[4,[5,6,7]]]],8,9]";
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
            public Type Type => typeof(Item);
            public bool TryDequeue(out IData data)
            {
                return this.Parent.TryDequeue(out data);
            }
        }
        public class List : IData
        {
            public Type Type => typeof(List);
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
                return this.datas.TryDequeue(out data) 
                    ;
            }
        }

        public long SolveNext()
        {
            return 0;
        }
    }
}