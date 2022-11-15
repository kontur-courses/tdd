using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public static class PointExtensions
    {
        public static double GetDistanceTo(this Point thisPoint, Point other)
        {
            return Math.Sqrt((thisPoint.X - other.X) * (thisPoint.X - other.X) +
                             (thisPoint.Y - other.Y) * (thisPoint.Y - other.Y));
        }

        public static double GetDistanceTo(this PointF thisPoint, PointF other)
        {
            return Math.Sqrt((thisPoint.X - other.X) * (thisPoint.X - other.X) +
                             (thisPoint.Y - other.Y) * (thisPoint.Y - other.Y));
        }

        public static Point ToPoint(this PointF pointF)
        {
            return new((int) Math.Round(pointF.X), (int) Math.Round(pointF.Y));
        }

        public static PointF AsPoint(this Point point)
        {
            return new(point.X, point.Y);
        }

        public static IEnumerable<PointF> ScatterPointsBySpiralAround(
            this Point center,
            double approximatePointSpacing,
            double startAngle = 0)
        {
            return ScatterPointsBySpiralAround(center.AsPoint(), approximatePointSpacing, startAngle);
        }

        public static IEnumerable<PointF> ScatterPointsBySpiralAround(
            this PointF center,
            double approximatePointSpacing,
            double startAngle = 0)
        {
            var polarArgument = startAngle;
            var polarRadius = approximatePointSpacing;

            yield return new PointF(
                (float) (center.X + polarRadius * Math.Cos(polarArgument)),
                (float) (center.Y + polarRadius * Math.Sin(polarArgument))
            );

            while (true)
            {
                polarArgument += Math.Atan(approximatePointSpacing / polarRadius);
                polarRadius = ((polarArgument - startAngle) / 2 / Math.PI + 1) * approximatePointSpacing;

                yield return new PointF(
                    (float) (center.X + polarRadius * Math.Cos(polarArgument)),
                    (float) (center.Y + polarRadius * Math.Sin(polarArgument))
                );
            }
        }
    }
}