using System.Collections.Generic;
using System.Drawing;

namespace TagCloud.Geometry
{
    public static class RectangleExtension
    {
        public static bool IsIntersectsWithAny(this Rectangle rect, List<Rectangle> rectangles)
        {
            foreach (var rectangle in rectangles)
                if (rect.IntersectsWith(rectangle))
                    return true;

            return false;
        }

        public static void MoveMiddlePointToCurrentLocation(this Rectangle rect)
        {
            rect.Location = new Point(rect.X - rect.Width / 2, rect.Y - rect.Height / 2);
        }
    }
}