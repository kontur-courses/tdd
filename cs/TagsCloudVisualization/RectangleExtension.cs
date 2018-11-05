using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public static class RectangleExtension
    {
        public static Rectangle Enlarge(this Rectangle rectangle, int delta)
        {
            return new Rectangle(rectangle.X - delta, rectangle.Y - delta, rectangle.Width + 2 * delta,
                rectangle.Height + 2 * delta);
        }

        public static Point GetCenter(this Rectangle rectangle)
        {
            int xCenter = rectangle.X + rectangle.Width / 2;
            int yCenter = rectangle.Y + rectangle.Height / 2;
            return new Point(xCenter, yCenter);
        }

        public static bool IntersectsWithAny(this Rectangle rectangle, IEnumerable<Rectangle> otherRectangles)
        {
            return otherRectangles.Any(rect => rectangle.IntersectsWith(rect));
        }
    }
}
