using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public static class RectangleExtension
    {
        public static bool IntersectsWith(this Rectangle rectangle, List<Rectangle> rectangleList)
        {
            return rectangleList.Any(r => r.IntersectsWith(rectangle));
        }

        public static double MaxDistanceToPoint(this Rectangle rectangle, Point point)
        {
            var corners = new List<Point>()
            {
                rectangle.Location,
                new Point(rectangle.X, rectangle.Y + rectangle.Height),
                new Point(rectangle.X + rectangle.Width, rectangle.Y),
                new Point(rectangle.X + rectangle.Width, rectangle.Y + rectangle.Height)
            };
            return corners.Max(p => Math.Sqrt(Math.Pow(p.X - point.X, 2) + Math.Pow(p.Y - point.Y, 2)));
        }
    }
}