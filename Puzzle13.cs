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

                var a = true;
                IData left = packetOne;
                IData right = packetTwo;
                int totalCount = 0;
                while(a)
                {
                    totalCount++;
                    var compareStrategy = this.GetCompareStrategy((left.GetType(), right.GetType()));
                    if(compareStrategy(left, right))
                    {
                        //left is smaller. correct order
                        a = false;
                        result.Add( totalPacketsInRightPosition);
                    }

                    //have to get again because mixedStategy is type order dependent
                    else if(this.GetCompareStrategy((right.GetType(), left.GetType()))(right, left))
                    {
                        //right is smaller. wrong order
                        a = false;
                    }
                    else
                    {
                        //keep going

                        //kan vara så att vi  går för mycket på djupen här.. 
                        //om vi kommer hit med Item som typ så smäller datas.
                        var anyLeft = left.TryDequeue(out left);
                        var anyRight = right.TryDequeue(out right);
                        if(anyLeft != anyRight)
                        {
                            a = false;
                            if(anyRight)
                            {
                                result.Add( totalPacketsInRightPosition);
                            }
                        }
                        if(anyLeft == false && anyRight == false)
                        {
                            throw new NotImplementedException("hamnar jag här har jag gjort fel");
                            //borde inte komma hit eftersom att då är båda slut..

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
            return result.Sum();
        }
        bool CompareNumber(Item left, Item right)
        {
            //left is smaller and in correct order
            return left.Number < right.Number;
        }
        bool CompareList(List left, List right)
        {
            return false;
        }
        bool CompareMixed(List list, Item item)
        {
            //convert item to list and compare.
            // var childList = list.AddList();
            // childList.AddNumber(item.Number);

            list.AddNumber(item.Number);
            return false;
        }

        Func<IData,IData,bool> GetCompareStrategy((Type a, Type b) bb)
        {
            var stratigies = new Dictionary<(Type, Type), Func<IData,IData,bool>>()
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
                else if(float.TryParse(newInput, out float number))
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
            public Queue<IData> datas { get; }
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

            public void AddNumber(int a)
            {
                var item = new Item(a);
                item.Parent = this;
                this.datas.Enqueue(item);
            }
            public bool TryDequeue(out IData data)
            {
                return this.datas.TryDequeue(out data) 
                    || (this.Parent?.TryDequeue(out data) 
                        ?? false);
            }
        }

        public long SolveNext()
        {
            return 0;
        }
    }
}