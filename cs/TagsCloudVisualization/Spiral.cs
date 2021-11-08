using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class Spiral : IPointGenerator
    {
        private Dictionary<Size, int> sizeToParameter = new Dictionary<Size, int>();

        public IEnumerable<PointF> GetPointsIn(PointF center, Size size)
        {
            foreach (var point in GetPoints(size))
            {
                yield return new PointF(point.X + center.X, point.Y + center.Y);
            }
        }
        public IEnumerable<PointF> GetPoints(Size tagSize)
        {
            float parameter = 0;
            if (sizeToParameter.ContainsKey(tagSize))
                parameter = sizeToParameter[tagSize];
            foreach (var p in GetArchimedeanSpiral(0, 3f))
            {
                yield return p;
            }    
        }


        private static void PolarToCartesian(float r, float angle,
            out float x, out float y)
        {
            x = (float)(r * Math.Cos(angle));
            y = (float)(r * Math.Sin(angle));
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
            /*for (;; angle+=pitch*p/(2*Math.PI))
            {
                PolarToCartesian(p, pitch, out var x, out var y);
            }*/
        }
    }
}