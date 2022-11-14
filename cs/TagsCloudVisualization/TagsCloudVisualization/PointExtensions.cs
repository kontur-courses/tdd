using System;
using System.Drawing;

namespace TagsCloudVisualization
{
    public static class PointExtensions
    {
        public static double GetDistanceTo(this Point thisPoint, Point other)
        {
            return Math.Sqrt((thisPoint.X - other.X) * (thisPoint.X - other.X) -
                             (thisPoint.Y - other.Y) * (thisPoint.Y - other.Y));
        }
    }
}