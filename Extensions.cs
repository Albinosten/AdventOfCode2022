using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

namespace AdventOfCode2022
{
    public static class Extensions
    {
        public static IEnumerable<T> TakeWhileInclusive<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            foreach(T item in source)
            {
                if(predicate(item)) 
                {
                    yield return item;
                }
                else
                {
                    yield return item;
                    yield break;
                }
            }
        }
        public static IEnumerable<T> TakeWhileIncludingSelf<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            var list = source.ToList();
            if(list.Count > 0)
            {
                yield return list[0];
                for(int i = 1; i < list.Count; i++)
                {
                    if(predicate(list[i]))
                    {
                        yield return list[i];
                    }
                    else
                    {
                        yield break;
                    }
                }
            }
        }
        public static IEnumerable<IList<T>> SplitList<T>(this IList<T> lines, int nSize)  
        {        
            for (int i = 0; i < lines.Count; i += nSize) 
            { 
                yield return lines.ToList().GetRange(i, Math.Min(nSize, lines.Count - i)); 
            }  
        }
        public static long Multiply(this IEnumerable<long> input)
        {
            var lines = input.ToList();
            long result = lines[0];
            for (int i = 1; i < lines.Count; i ++) 
            { 
                result = result*lines[i];
            }  
            return result;
        }
        public static int Multiply(this IEnumerable<int> input)
        {
            return (int)input.Select(x => (long)x).Multiply();
        }

        public static IEnumerable<IEnumerable<T>> GetPermutations<T>(this IEnumerable<T> list, int length)
        {
            if (length == 1) return list.Select(t => new T[] { t });

            return GetPermutations(list, length - 1)
                .SelectMany(t => list.Where(e => !t.Contains(e)),
                    (t1, t2) => t1.Concat(new T[] { t2 }));
        }
    }
}