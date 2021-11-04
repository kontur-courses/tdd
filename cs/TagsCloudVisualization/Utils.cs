using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    internal static class Utils
    {
        internal static double DistanceTo(this Point from, Point to)
        {
            return Math.Sqrt((from.X - to.X) * (from.X - to.X) + (from.Y - to.Y) * (from.Y - to.Y));
        }

        internal static double Length(this Point from)
        {
            return Math.Sqrt(from.X * from.X + from.Y * from.Y);
        }

        internal static Point GetCenter(this Rectangle rectangle)
        {
            return rectangle.Location + rectangle.Size / 2;
        }

        internal static TSource MinBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            using var sourceIterator = source.GetEnumerator();
            if (!sourceIterator.MoveNext())
                return default;

            var min = sourceIterator.Current;
            var minKey = selector(min);
            while (sourceIterator.MoveNext())
            {
                var candidate = sourceIterator.Current;
                var candidateProjected = selector(candidate);
                if (Comparer<TKey>.Default.Compare(candidateProjected, minKey) < 0)
                {
                    min = candidate;
                    minKey = candidateProjected;
                }
            }

            return min;
        }
    }
}
