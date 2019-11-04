using System;
using System.Drawing;

namespace TagsCloudVisualization
{
    internal static class Geometry
    {
        public static Point PolarToCartesion(double ro, double phi)
        {
            return new Point((int) (ro * Math.Cos(phi)), (int) (ro * Math.Sin(phi)));
        }

        public static double GetLengthFromRectCenterToBorder(Rectangle rect, Point point)
        {
            if (rect.Width < 0 || rect.Height < 0) throw new ArgumentException();

            double dx = Math.Abs(rect.X - point.X);
            double dy = Math.Abs(rect.Y - point.Y);
            var xOffset = rect.Width / 2;
            var yOffset = rect.Height / 2;

            if (dx <= xOffset && dy <= yOffset) return 0;

            if (dx > xOffset && dy > yOffset)
            {
                var k = xOffset / dx;
                dx = k * dx;
                dy = k * dy;
            }

            var hypothenuse = Math.Sqrt(dx * dx + dy * dy);
            return dx <= xOffset ? hypothenuse * yOffset / dy : hypothenuse * xOffset / dx;
        }

        public static Point ShiftPointBySizeOffsets(Point point, Size size)
        {
            if (size.Width < 0 || size.Height < 0) throw new ArgumentException();
            return new Point(point.X - size.Width / 2, point.Y - size.Height / 2);
        }
    }
}