using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public static class SpiralPath
    {
        private static PointF ArchimedeanSpiral(double spiralStepCoefficient, double angleRadians)
        {
            var radius = spiralStepCoefficient / (2 * Math.PI) * angleRadians;
            var x = (float) (radius * Math.Cos(angleRadians));
            var y = (float) (radius * Math.Sin(angleRadians));

            return new PointF(x, y);
        }

        public static IEnumerable<PointF> GetSpiralPointsF(double spiralStepCoefficient, double angleStepRadians)
        {
            for (var angle = 0d; Math.Abs(angle) < double.MaxValue; angle += angleStepRadians)
            {
                yield return ArchimedeanSpiral(spiralStepCoefficient, angle);
            }
        }

        public static IEnumerable<Point> GetSpiralPoints(double spiralStepCoefficient, double angleStepRadians)
        {
            return GetSpiralPointsF(spiralStepCoefficient, angleStepRadians).Select(pointF => new Point((int) pointF.X, (int) pointF.Y));
        }
    }
}
