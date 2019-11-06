using System;
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

        public static Rectangle GetClosestToThePointRectangle(Point point, Rectangle[] rectangles)
        {
            var closestRectangle = rectangles[0];
            var shortestDistance = GetDistanceFromRectangleToPoint(point, closestRectangle);
            foreach (var rectangle in rectangles)
            {
                var distance = GetDistanceFromRectangleToPoint(point, rectangle);
                if (distance >= shortestDistance)
                    continue;
                shortestDistance = distance;
                closestRectangle = rectangle;
            }

            return closestRectangle;
        }
    }
}
