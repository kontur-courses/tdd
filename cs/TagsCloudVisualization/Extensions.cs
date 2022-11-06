using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public static class Extensions
    {
        public static Size Multiply(this Size size, double number)
        {
            return new Size((int)(size.Width * number), (int)(size.Height * number));
        }

        public static Point GetCenter(this Rectangle rectangle)
        {
            Point topLeftPoint = rectangle.Location;
            return new Point(topLeftPoint.X + rectangle.Width / 2, topLeftPoint.Y + rectangle.Height / 2);
        }

        public static bool IntersectsWith(this Rectangle rectangle, IReadOnlyList<Rectangle> otherRectangles)
        {
            return otherRectangles.Any(rect => rect.IntersectsWith(rectangle));
        }
        
        public static void DrawCircle(this Graphics graphics, Pen pen, Point center, float radius)
        {
            graphics.DrawEllipse(pen, center.X - radius, center.Y - radius,
                radius + radius, radius + radius);
        }
    }
}