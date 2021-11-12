using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagCloud.Geometry
{
    public static class RectangleExtension
    {
        public static bool IntersectsWithAny(this Rectangle rect, List<Rectangle> rectangles)
        {
            return rectangles.Any(rectangle => rect.IntersectsWith(rectangle));
        }

        public static void MoveMiddlePointToCurrentLocation(this Rectangle rect)
        {
            rect.Location = rect.Location.MovePoint(rect.X - rect.Width / 2,
                rect.Y - rect.Height / 2);
        }

        public static IEnumerable<Point> GetCorners(this Rectangle rect)
        {
            yield return new Point(rect.X, rect.Y);
            yield return new Point(rect.X + rect.Width, rect.Y);
            yield return new Point(rect.X, rect.Y + rect.Height);
            yield return new Point(rect.X + rect.Width, rect.Y + rect.Height);
        }

        public static double GetLongestDistanceFromPoint(this Rectangle rect, Point point)
        {
            return rect.GetCorners().Max(corner => corner.GetDistanceTo(point));
        }
    }
}