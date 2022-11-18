using System;
using System.Drawing;

namespace TagsCloudVisualization.Utils
{
    public static class PointExtensions
    {
        public static double GetDistanceTo(this Point thisPoint, Point other)
        {
            return thisPoint.AsPointF().GetDistanceTo(other.AsPointF());
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

        public static PointF AsPointF(this Point point)
        {
            return new(point.X, point.Y);
        }
    }
}