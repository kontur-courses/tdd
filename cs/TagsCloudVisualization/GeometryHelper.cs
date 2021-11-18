using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    internal static class GeometryHelper
    {
        public static float GetDistanceBetweenPoints(PointF p1, PointF p2)
        {
            var x = p2.X - p1.X;
            var y = p2.Y - p1.Y;

            return MathF.Sqrt(x * x + y * y);
        }

        public static Point GetRectangleCenter(Rectangle rect)
        {
            var x = rect.X + (int)MathF.Round(rect.Width / 2.0f);
            var y = rect.Y + (int)MathF.Round(rect.Height / 2.0f);

            return new Point(x, y);
        }

        public static Point GetRectangleLocationFromCenter(Point rectCenter, Size size)
        {
            var x = (int)MathF.Floor(rectCenter.X - size.Width / 2.0f);
            var y = (int)MathF.Floor(rectCenter.Y - size.Height / 2.0f);

            return new Point(x, y);
        }

        public static Point GetPointOnCircle(Point center, float radius, float angle)
        {
            var x = center.X + (int)MathF.Round(MathF.Cos(angle) * radius);
            var y = center.Y - (int)MathF.Round(MathF.Sin(angle) * radius);

            return new Point(x, y);
        }

        public static Size GetRectanglesCommonSize(IEnumerable<Rectangle> rectangles, int framing = 500)
        {
            var commonRectangle = Rectangle.Empty;
            var frame = new Size(framing, framing);
            
            rectangles.Aggregate(commonRectangle, Rectangle.Union);

            return commonRectangle.Size + frame;
        }

        public static IEnumerable<Point> GetAllPointIntoRectangle(Rectangle rectangle)
        {
            if (rectangle.Size.IsEmpty)
                return ArraySegment<Point>.Empty;

            var xRange = Enumerable.Range(rectangle.Left + 1, rectangle.Right - rectangle.Left);
            var yRange = Enumerable.Range(rectangle.Top + 1, rectangle.Bottom - rectangle.Top);

            return xRange.SelectMany(x => yRange.Select(y => new Point(x, y)));
        }
    }
}