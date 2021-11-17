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

        public static PointF GetRectangleCenter(Rectangle rect)
        {
            var x = rect.X + rect.Width / 2.0f;
            var y = rect.Y + rect.Height / 2.0f;

            return new PointF(x, y);
        }

        public static Point GetRectangleLocationFromCenter(PointF rectCenter, Size size)
        {
            var x = (int)Math.Floor(rectCenter.X - size.Width / 2.0);
            var y = (int)Math.Floor(rectCenter.Y - size.Height / 2.0);

            return new Point(x, y);
        }

        public static PointF GetPointOnCircle(Point Center, float radius, float angle)
        {
            var x = Center.X + MathF.Cos(angle) * radius;
            var y = Center.Y - MathF.Sin(angle) * radius;

            return new PointF(x, y);
        }

        public static Size GetRectanglesCommonSize(IEnumerable<Rectangle> rectangles, int framing = 500)
            => rectangles.Aggregate(new Rectangle(), Rectangle.Union).Size
               + new Size(framing, framing);

        public static IEnumerable<Point> GetAllPointIntoRectangle(Rectangle rectangle)
        {
            if(rectangle.Size.IsEmpty)
                return ArraySegment<Point>.Empty;
            
            var xRange = Enumerable.Range(rectangle.Left + 1, rectangle.Right - rectangle.Left);
            var yRange = Enumerable.Range(rectangle.Top + 1, rectangle.Bottom - rectangle.Top);

            return xRange.SelectMany(x => yRange.Select(y => new Point(x, y)));
        }
    }
}