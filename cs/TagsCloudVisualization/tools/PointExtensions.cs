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

        public static Point Sub(this Point a, Point b) => new Point(a.X - b.X, a.Y - b.Y);

        public static Point Add(this Point a, Point b) => new Point(a.X + b.X, a.Y + b.Y);

        public static Point Scale(this Point a, int k) => new Point(a.X * k, a.Y * k);

        public static int PMul(this Point a, Point b) => a.X * b.Y - a.Y * b.X;
    }
}