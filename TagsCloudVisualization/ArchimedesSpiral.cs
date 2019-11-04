using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class ArchimedesSpiral : IEnumerable<PointF>
    {
        public readonly float A;
        public readonly PointF Center;
        public const float DeltaAngle = (float)(5 * Math.PI / 180);


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
                (x, y) = TransformPolarToCartesian(r, theta);
                x += center.X;
                y += center.Y;
                yield return new PointF(x, y);
            }
        }

        public static Tuple<float, float> TransformPolarToCartesian(float r, float theta)
        {
            return new Tuple<float, float>(
                (float)(r * Math.Cos(theta)),
                (float)(r * Math.Sin(theta)));
        }

        public static Tuple<float, float> TransformCartesianToPolar(float x, float y)
        {
            var theta = (float)Math.Atan2(y, x);
            var r = (float)Math.Sqrt(x * x + y * y);
            return new Tuple<float, float>(r, theta);
        }

        public static bool TryFindPreviousSpinPoint(PointF currentSpinPoint, float a, out PointF previousSpinPoint)
        {
            var (r, theta) =
                TransformCartesianToPolar(currentSpinPoint.X, currentSpinPoint.Y);
            theta -= (float) (2 * Math.PI * a);
            if (theta < 0)
            {
                previousSpinPoint = new PointF(0, 0);
                return false;
            }
            r = theta * a;
            var (x, y) = TransformPolarToCartesian(r, theta);
            previousSpinPoint = new PointF(x, y);
            return true;
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
