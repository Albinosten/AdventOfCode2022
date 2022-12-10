using System;
using System.Collections.Generic;

namespace AdventOfCode2022
{
    public static class ExtensionsPuzzle8
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
    }
}