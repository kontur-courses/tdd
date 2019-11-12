using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class ArchimedesSpiral : IEnumerable<PointF>
    {
        public const float DeltaAngle = (float)(5 * Math.PI / 180);

        private readonly float A;
        private readonly PointF Center;

        public ArchimedesSpiral(float a, PointF center)
        {
            A = a;
            Center = center;
        }

        private static IEnumerable<PointF> GetSpiralPoints(PointF center, float a)
        {
            for (float theta = 0; ; theta += DeltaAngle)
            {
                var r = a * theta;
                float x, y;
                (x, y) = PointConverter.TransformPolarToCartesian(r, theta);
                x += center.X;
                y += center.Y;
                yield return new PointF(x, y);
            }
        }

        public IEnumerator<PointF> GetEnumerator()
        {
            return GetSpiralPoints(Center, A).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
