using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization.Utils
{
    public static class RectangleExtensions
    {
        public static IEnumerable<Point> GetVertices(this Rectangle rectangle)
        {
            var upperLeft = rectangle.Location;
            return Enumerable.Empty<Point>()
                .Append(upperLeft)
                .Append(upperLeft + new Size(0, rectangle.Height))
                .Append(upperLeft + new Size(rectangle.Width, 0))
                .Append(upperLeft + new Size(rectangle.Width, rectangle.Height));
        }

        public static Rectangle NewRectangle(Point center, Size size)
        {
            return new(center - size / 2, size);
        }
    }
}