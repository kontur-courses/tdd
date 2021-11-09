using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public static class Utils
    {
        public static double DistanceTo(this Point from, Point to)
        {
            return Math.Sqrt((from.X - to.X) * (from.X - to.X) + (from.Y - to.Y) * (from.Y - to.Y));
        }

        public static double Length(this Point from)
        {
            return Math.Sqrt(from.X * from.X + from.Y * from.Y);
        }

        public static Point GetCenter(this Rectangle rectangle)
        {
            return rectangle.Location + rectangle.Size / 2;
        }

        public static IEnumerable<Point> GetAllPoints(this Rectangle rectangle)
        {
            yield return rectangle.Location;
            yield return new Point(rectangle.Left, rectangle.Bottom);
            yield return new Point(rectangle.Right, rectangle.Top);
            yield return new Point(rectangle.Right, rectangle.Bottom);
        }

        public static int GetArea(this Size size)
        {
            return size.Width * size.Height;
        }

        public static TSource MinBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector)
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
