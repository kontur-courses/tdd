using System;
using System.Drawing;

namespace TagsCloudVisualization
{
    public static class LinearMath
    {
        public static double GetDiagonal(Size size)
        {
            return Math.Sqrt(size.Width * size.Width + size.Height * size.Height);
        }

        public static Point PolarToCartesian(Point center, double radius, double angle)
        {
            var x = center.X + (int)(radius * (float)Math.Cos(angle));
            var y = center.Y + (int)(radius * (float)Math.Sin(angle));
            return new Point(x, y);
        }

        public static double DistanceBetween(Point from, Point to)
        {
            var x = from.X - to.X;
            var y = from.Y - to.Y;
            return Math.Sqrt(x * x + y * y);
        }
    }
}
