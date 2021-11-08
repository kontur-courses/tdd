using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization.PointGenerator
{
    class Circle : IPointGenerator
    {
        public IEnumerable<PointF> GetPoints(PointF center)
        {
            return GetPointsFrom(0, center);
        }

        public static IEnumerable<PointF> GetPointsFrom(int radius, PointF center)
        {
            while (true)
            {
                foreach (var p in GetPointsInRadius(radius++, center))
                    yield return p;
            }
        }

        private static IEnumerable<PointF> GetPointsInRadius(int radius, PointF center)
        {
            return GetPointsInRadiusInZeroCenter(radius)
                .Select(point => new PointF(point.X + center.X, point.Y + center.Y));
        }

        private static IEnumerable<PointF> GetPointsInRadiusInZeroCenter(int radius)
        {
            for (float x = -radius; x < radius; x += 0.5f)
            {
                yield return new PointF(x, (float)Math.Sqrt(radius * radius - x * x));
                yield return new PointF(x, (float)-Math.Sqrt(radius * radius - x * x));
            }
        }
    }
}