using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace CloudLayouter
{
    public static class RectangleExtensions
    {
        public static bool IntersectsWith(this Rectangle rectangle, IEnumerable<Rectangle> rectangles)
            => rectangles.Any(rectangle.IntersectsWith);
        
        internal static Point GetCenter(this Rectangle rectangle)
            => rectangle.Location + rectangle.Size / 2;
    }
}