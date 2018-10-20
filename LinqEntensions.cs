using System;
using System.Collections.Generic;
namespace System.Linq
{
    public static class LinqEntensions
    {
        // ForEach
        public static IEnumerable<TSource> ForEach<TSource>(this IEnumerable<TSource> source,
            Action<TSource> action)
        {
            foreach (TSource item in source)
                action(item);

            return source;
        }

      //  Foreach with index
        public static IEnumerable<TSource> ForEach<TSource>(this IEnumerable<TSource> source,
            Action<TSource, int> action)
        {
            int index = 0;
            foreach (TSource item in source)
                action(item, index++);

            return source;
        }
        // Foreach with index and previous item
        public static IEnumerable<TSource> ForEach<TSource>(this IEnumerable<TSource> source,
            Action<TSource, int, TSource> action)
        {
            int index = 0;
            TSource previousItem = default(TSource);
            foreach (TSource item in source)
            {
                action(item, index++, previousItem);
                previousItem = item;
            }

            return source;
        }

        // Foreach with index and 2 previous items
        public static IEnumerable<TSource> ForEach<TSource>(this IEnumerable<TSource> source,
            Action<TSource, int, TSource, TSource> action)
        {
            int index = 0;
            TSource previousItem1 = default(TSource);
            TSource previousItem2 = default(TSource);
            foreach (TSource item in source)
            {
                action(item, index++, previousItem1, previousItem2);
                previousItem2 = previousItem1;
                previousItem1 = item;
            }

            return source;
        }
        // Breakable ForEach
        public static IEnumerable<TSource> ForEach<TSource>(this IEnumerable<TSource> source,
            Func<TSource, bool> action)
        {
            foreach (TSource item in source)
            {
                if (!action(item))
                    break;
            }

            return source;
        }
        // ForEach with index and previous result
        /*int[] arr = new int[] { 0, 1, 2, 3, 4, 5 };

 // ForEach with index and previous result
 arr.ForEach<int, int>((x, index, previousResult) =>
 {
 int result = 10 * x;
 Console.WriteLine("result " + result + ", previous result " + previousResult);
 return result;
 });

 // Running sum
 arr.ForEach<int, int>((x, index, previousResult) =>
 {
 int runningSum = previousResult + x;
 Console.Write(runningSum + " ");
 return runningSum;
 });*/
        public static IEnumerable<TSource> ForEach<TSource, TResult>(this IEnumerable<TSource> source,
            Func<TSource, int, TResult, TResult> action)
        {
            int index = 0;
            TResult previousResult = default(TResult);
            foreach (TSource item in source)
                previousResult = action(item, index++, previousResult);

            return source;

        }

    }
}