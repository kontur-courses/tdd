using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization.Extensions
{
    public static class IEnumerableExtensions
    {
        
        public static IEnumerable<TOut> ForAllPairs<TIn,TOut>(this IList<TIn> coll, Func<TIn,TIn,TOut> func)
        {
            for (int i = 0; i < coll.Count; i++)
            for (int j = 0; j < i; j++)
                yield return func(coll[i], coll[j]);
        }
        
        public static void ForAllPairs<TIn>(this IList<TIn> coll, Action<TIn,TIn> func)
        {
            for (int i = 0; i < coll.Count; i++)
            for (int j = 0; j < i; j++)
                func(coll[i], coll[j]);
        }
        
        
        public static void ForAllPairs<TIn>(this IEnumerable<TIn> enumerable, Action<TIn,TIn> func)
        {
            var gen = enumerable.GetEnumerator();
            if (!gen.MoveNext())
                return;
            var list = new List<TIn>{gen.Current};
            while (gen.MoveNext())
                foreach (var el in list)
                    func(gen.Current, el);
            gen.Dispose();
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
    }
}