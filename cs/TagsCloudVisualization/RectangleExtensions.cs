using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public static class RectangleExtensions
    {
        public static bool IntersectsWith(this Rectangle rectangle, ICollection<Rectangle> rectangles)
        {
            return rectangles.Any(rectangle.IntersectsWith);
        }
    }
}