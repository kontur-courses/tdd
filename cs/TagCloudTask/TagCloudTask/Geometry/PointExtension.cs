using System;
using System.Drawing;

namespace TagCloudTask.Geometry
{
    public static class PointExtension
    {
        public static Point MovePoint(this Point point, int x, int y)
        {
            return new Point(point.X + x, point.Y + y);
        }

        public static double GetDistanceTo(this Point from, Point to)
        {
            return Math.Sqrt((to.X - from.X) * (to.X - from.X)
                             + (to.Y - from.Y) * (to.Y - from.Y));
        }
    }
}