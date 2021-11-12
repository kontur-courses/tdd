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
            rect.Location = new Point(rect.X - rect.Width / 2, rect.Y - rect.Height / 2);
        }
    }
}