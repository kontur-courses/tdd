using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    class Circle
    {
        public static IEnumerable<Point> GetPointsFrom(int radius, Point center)
        {
            while (true)
            {
                foreach (var p in GetPointsInRadius(radius++, center))
                    yield return p;
            }
        }

        private static IEnumerable<Point> GetPointsInRadius(int radius, Point center)
        {
            return Enumerable.Select<Point, Point>(GetPointsInRadiusInZeroCenter(radius), point => new Point(point.X + center.X, point.Y + center.Y));
        }

        private static IEnumerable<Point> GetPointsInRadiusInZeroCenter(int radius)
        {
            yield return new Point(-radius, 0);
            for (var x = -radius + 1; x < radius; x++)
            {
                yield return new Point(x, (int)Math.Sqrt(radius * radius - x * x));
                yield return new Point(x, -(int)Math.Sqrt(radius * radius - x * x));
            }

            yield return new Point(radius, 0);
        }
    }
}