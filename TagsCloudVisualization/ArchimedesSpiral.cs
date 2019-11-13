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

        private IEnumerable<PointF> GetSpiralPoints()
        {
            for (float theta = 0; ; theta += DeltaAngle)
            {
                var r = A * theta;
                float x, y;
                (x, y) = PointConverter.TransformPolarToCartesian(r, theta);
                x += Center.X;
                y += Center.Y;
                yield return new PointF(x, y);
            }
        }

        public IEnumerator<PointF> GetEnumerator()
        {
            return GetSpiralPoints().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
