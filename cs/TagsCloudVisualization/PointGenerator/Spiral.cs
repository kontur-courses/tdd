using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization.PointGenerator
{
    public class Spiral : IPointGenerator
    {
        public IEnumerable<PointF> GetPoints(PointF center)
        {
            return GetArchimedeanSpiral(0, 3f).Select(point => new PointF(point.X + center.X, point.Y + center.Y));
        }

        private static IEnumerable<PointF> GetArchimedeanSpiral(float angle, float pitch)
        {
            while (true)
            {
                var p = (float)(pitch * angle / (2 * Math.PI));
                PolarToCartesian(p, angle, out var x, out var y);
                yield return new PointF(x, y);
                angle += 0.5f;
            }
        }

        private static void PolarToCartesian(float r, float angle,
            out float x, out float y)
        {
            x = (float)(r * Math.Cos(angle));
            y = (float)(r * Math.Sin(angle));
        }
    }
}