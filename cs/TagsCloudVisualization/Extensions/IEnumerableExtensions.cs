using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization.Extensions
{
    public static class IEnumerableExtensions
    {
        
        public static IEnumerable<TOut> ForAllPairs<TIn,TOut>(this IList<TIn> list, Func<TIn,TIn,TOut> func)
        {
            for (int i = 0; i < list.Count; i++)
            for (int j = 0; j < i; j++)
                yield return func(list[i], list[j]);
        }
        
        public static void ForAllPairs<TIn>(this IList<TIn> list, Action<TIn,TIn> func)
        {
            for (int i = 0; i < list.Count; i++)
            for (int j = 0; j < i; j++)
                func(list[i], list[j]);
        }
        
        
        public static void ForAllPairs<TIn>(this IEnumerable<TIn> enumerable, Action<TIn,TIn> func)
        {
            using (var enumerator = enumerable.GetEnumerator())
            {
                if (!enumerator.MoveNext())
                    return;
                var list = new List<TIn>{enumerator.Current};
                while (enumerator.MoveNext())
                    foreach (var el in list)
                        func(enumerator.Current, el);
            }
        }

        public static Rectangle EnclosingRectangle(this IEnumerable<Rectangle> enumerable) =>
            enumerable.Aggregate((x, y) =>
            {
                var left = Math.Min(x.Left, y.Left);
                var top = Math.Min(x.Top, y.Top);
                var right = Math.Max(x.Right, y.Right);
                var bottom = Math.Max(x.Bottom, y.Bottom);
                return new Rectangle(left,top,right-left,bottom-top);
            });
        
        public static void Times(this int count, Action act)
        {
            for (int i = 0; i < count; i++)
                act();
        }
        
        public static IEnumerable<T> Times<T>(this int count, Func<T> act)
        {
            for (int i = 0; i < count; i++)
                yield return act();
        }

        public static IEnumerable<Tuple<T, int>> CountUnique<T>(this IEnumerable<T> eunmerable) =>
            eunmerable.GroupBy(x => x).Select(x => Tuple.Create(x.Key, x.Count()));
    }
}