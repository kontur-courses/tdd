using System;
using System.Drawing;

namespace TagsCloudVisualization
{
    public static class PointExtensions
    {
        public static double GetDistanceTo(this Point point, Point anotherPoint)
        {
            var dx = point.X - anotherPoint.X;
            var dy = point.Y - anotherPoint.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        public static Point Add(this Point point, Point anotherPoint)
        {
            var x = point.X + anotherPoint.X;
            var y = point.Y + anotherPoint.Y;
            return new Point(x, y);
        }
    }
}