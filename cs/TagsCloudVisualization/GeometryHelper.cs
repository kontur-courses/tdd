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

        public static PointF GetRectangleCentre(Rectangle rect)
        {
            var x = rect.X + rect.Width / 2.0f;
            var y = rect.Y + rect.Height / 2.0f;

            return new PointF(x, y);
        }

        public static Point GetRectangleLocationFromCentre(PointF rectCentre, Size size)
        {
            var x = (int)Math.Floor(rectCentre.X - size.Width / 2.0);
            var y = (int)Math.Floor(rectCentre.Y - size.Height / 2.0);

            return new Point(x, y);
        }

        public static PointF GetPointOnCircle(Point centre, float radius, float angle)
        {
            var x = centre.X + MathF.Cos(angle) * radius;
            var y = centre.Y - MathF.Sin(angle) * radius;

            return new PointF(x, y);
        }

        public static Size GetRectanglesCommonSize(IEnumerable<Rectangle> rectangles, int framing = 500)
            => rectangles.Aggregate(new Rectangle(), Rectangle.Union).Size
               + new Size(framing, framing);

        public static IEnumerable<Point> GetAllPointIntoRectangle(Rectangle rectangle)
        {
            var range1 = Enumerable.Range(rectangle.Left + 1, rectangle.Right - rectangle.Left);
            var range2 = Enumerable.Range(rectangle.Top + 1, rectangle.Bottom - rectangle.Top);

            return range1.SelectMany(r1 => range2.Select(r2 => new Point(r1, r2)));
        }
    }
}