using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public static class RectangleExtensions
    {
        public static bool IntersectsWith(this Rectangle rect, IEnumerable<Rectangle> rectangles)
        {
            return rectangles.Any(i => i.IntersectsWith(rect));
        }

        public static Point GetMiddlePoint(this Rectangle rect)
        {
            var x = rect.X + rect.Width / 2;
            var y = rect.Y + rect.Height / 2;
            return new Point(x, y);
        }

        public static Rectangle CreateRectangle(Point centerOfRectangle, Size rectangleSize)
        {
            var x = centerOfRectangle.X - rectangleSize.Width / 2;
            var y = centerOfRectangle.Y - rectangleSize.Height / 2;
            return new Rectangle(new Point(x, y), rectangleSize);
        }
    }
}