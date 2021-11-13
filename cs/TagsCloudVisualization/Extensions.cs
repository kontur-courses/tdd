using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

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
        
        public static IEnumerable<(PointF, PointF)> GetSegments(this RectangleF rectangle)
        {
            var vertexes = rectangle.GetPoints().ToArray();
            for (var i = 0; i < vertexes.Length - 1; i++)
            {
                yield return (vertexes[i], vertexes[i + 1]);
            }
            yield return (vertexes[^1], vertexes[0]);
        }
        
        public static bool IsEmpty(this RectangleF rectangle, double precision)
        {
            return Math.Abs(rectangle.Width * rectangle.Height) < precision;
        }

        public static bool Contacts(this RectangleF rectangle, RectangleF other)
        {
            return Math.Abs(rectangle.Top - other.Top) < 0.01 && (Math.Abs(rectangle.Left - other.Right) < 0.01 ||
                                                                  Math.Abs(rectangle.Right - other.Left) < 0.01) ||
                Math.Abs(rectangle.Left - other.Left) < 0.01 && (Math.Abs(rectangle.Top - other.Bottom) < 0.01 ||
                                                                 Math.Abs(rectangle.Bottom - other.Top) < 0.01);
        }

        public static double DistanceTo(this PointF point, PointF other)
        {
            return Math.Sqrt((point.X - other.X)*(point.X - other.X) + (point.Y - other.Y) * (point.Y - other.Y));
        }
    }
}