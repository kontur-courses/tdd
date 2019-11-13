using System;
using System.Drawing;

namespace TagsCloudVisualization.tools
{
    internal static class PointExtensions
    {
        public static double Distance(this Point from, Point to)
        {
            var dx = to.X - from.X;
            var dy = to.Y - from.Y;

            return Math.Sqrt(dx * dx + dy * dy);
        }

        public static Point GetMiddlePoint(this Point from, Point to)
        {
            var dx = to.X - from.X;
            var dy = to.Y - from.Y;

            return new Point(from.X + dx / 2, from.Y + dy / 2);
        }
    }
}