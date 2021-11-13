using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public static class Extensions
    {
        public static IEnumerable<PointF> GetPoints(this RectangleF rectangle)
        {
            yield return rectangle.Location;
            yield return new PointF(rectangle.Right, rectangle.Top);
            yield return new PointF(rectangle.Right, rectangle.Bottom);
            yield return new PointF(rectangle.Left, rectangle.Bottom);
        }
        
        public static bool IsEmpty(this RectangleF rectangle, double precision)
        {
            return Math.Abs(rectangle.Width * rectangle.Height) < precision;
        }

        public static bool Contacts(this RectangleF rectangle, RectangleF other)
        {
            return rectangle.Top == other.Top && (rectangle.Left == other.Right || rectangle.Right == other.Left) ||
                rectangle.Left == other.Left && (rectangle.Top == other.Bottom || rectangle.Bottom == other.Top);
        }

        public static double DistanceTo(this PointF point, PointF other)
        {
            return Math.Sqrt((point.X - other.X)*(point.X - other.X) + (point.Y - other.Y) * (point.Y - other.Y));
        }
    }
}