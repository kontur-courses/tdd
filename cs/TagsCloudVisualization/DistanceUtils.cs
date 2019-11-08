using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class DistanceUtils
    {
        public static double GetDistanceFromPointToPoint(PointF firstPoint, PointF secondPoint)
        {
            var dx = firstPoint.X - secondPoint.X;
            var dy = firstPoint.Y - secondPoint.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        public static double GetDistanceFromSegmentToPoint(Point point, Segment segment)
        {
            var points = new[] { segment.Left, segment.GetCenter(), segment.Right };
            return points.Min(p => GetDistanceFromPointToPoint(point, p));
        }

        public static double GetDistanceFromRectangleToPoint(Point point, Rectangle rectangle)
        {
            return GeometryUtils
                .GetRectangleSides(rectangle)
                .Min(s => GetDistanceFromSegmentToPoint(point, s));
        }

        public static Rectangle GetClosestToThePointRectangle(Point point, IEnumerable<Rectangle> rectangles)
        {
            if (!rectangles.Any())
                throw new ArgumentException("There is no rectangles");

            return rectangles.Aggregate((closestRectangle, nextRectangle) =>
                GetDistanceFromRectangleToPoint(point, closestRectangle) < GetDistanceFromRectangleToPoint(point, nextRectangle)
                    ? closestRectangle
                    : nextRectangle);
        }
    }
}
