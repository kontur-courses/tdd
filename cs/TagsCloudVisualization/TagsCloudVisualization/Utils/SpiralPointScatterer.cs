using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization.Utils
{
    internal class SpiralPointScatterer
    {
        public static IEnumerable<PointF> Scatter(
            Point center,
            double approximatePointSpacing,
            double startAngle = 0)
        {
            return Scatter(center.AsPointF(), approximatePointSpacing, startAngle);
        }

        public static IEnumerable<PointF> Scatter(
            PointF center,
            double approximatePointSpacing,
            double startAngle = 0)
        {
            var polarArgument = startAngle;
            var polarRadius = approximatePointSpacing;

            while (true)
            {
                yield return new PointF(
                    (float) (center.X + polarRadius * Math.Cos(polarArgument)),
                    (float) (center.Y + polarRadius * Math.Sin(polarArgument))
                );

                polarArgument += Math.Atan(approximatePointSpacing / polarRadius);
                polarRadius = ((polarArgument - startAngle) / 2 / Math.PI + 1) * approximatePointSpacing;
            }
        }
    }
}