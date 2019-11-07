using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public static class RectangleExtensions
    {
        public static Point GetRectangleCenteredLocation(this Rectangle rectangle, Point point)
        {
            return new Point(point.X - rectangle.Width / 2, point.Y - rectangle.Height / 2);
        }

        public static bool IntersectsWithAny(this Rectangle rectangle, IEnumerable<Rectangle> rectangles)
        {
            return rectangles.Any(rect => rect.IntersectsWith(rectangle));
        }
    }
}