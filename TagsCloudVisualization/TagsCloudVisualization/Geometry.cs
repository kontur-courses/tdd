using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class Geometry
    {
        public static bool IsRectangleIntersection(Rectangle rectangle, Rectangle other)
        {
            return !(rectangle.X + rectangle.Width < other.X || other.X + other.Width < rectangle.X ||
                     rectangle.Y + rectangle.Height < other.Y || other.Y + other.Height < rectangle.Y);
        }

        public static double GetMaxDistanceToRectangle(Point point, Rectangle rectangle)
        {
            var maxRadius = 0.0;
            for (var x = rectangle.X; x <= rectangle.X + rectangle.Width; x += rectangle.Width)
            {
                for (var y = rectangle.Y; y <= rectangle.Y + rectangle.Height; y += rectangle.Height)
                {
                    var radius = GetDistanceBetweenPoints(point, new Point(x, y));
                    if (radius > maxRadius)
                        maxRadius = radius;
                }
            }

            return maxRadius;
        }

        public static double GetDistanceBetweenPoints(Point point, Point other)
        {
            return Math.Sqrt((point.X - other.X) * (point.X - other.X) +
                             (point.Y - other.Y) * (point.Y - other.Y));
        }

        public static double GetMaxDistanceToRectangles(Point point, IEnumerable<Rectangle> rectangles)
        {
            return rectangles.Select(rectangle => GetMaxDistanceToRectangle(point, rectangle)).Max();
        }
    }
}
