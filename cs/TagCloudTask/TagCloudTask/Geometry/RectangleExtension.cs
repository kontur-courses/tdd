using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagCloudTask.Geometry
{
    public static class RectangleExtension
    {
        public static bool IntersectsWithAny(this Rectangle rect, List<Rectangle> rectangles)
        {
            if (rectangles == null)
                throw new ArgumentException("rectangles list can't be null");
            return rectangles.Any(rectangle => rect.IntersectsWith(rectangle));
        }

        public static Rectangle MoveMiddlePointToCurrentLocation(this Rectangle rect)
        {
            var diffX = rect.X - rect.Width / 2;
            var diffY = rect.Y - rect.Height / 2;

            var newLocation = rect.Location.MovePoint(diffX, diffY);

            return new Rectangle(newLocation, rect.Size);
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