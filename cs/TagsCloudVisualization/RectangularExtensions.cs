using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public static class RectangularExtensions
    {
        public static IEnumerable<Point> GetRectangleNodes(this Rectangle rectangle)
        {
            yield return new Point(rectangle.X, rectangle.Y); //LU
            yield return new Point(rectangle.X + rectangle.Width, rectangle.Y); //RU
            yield return new Point(rectangle.X + rectangle.Width, rectangle.Y + rectangle.Height); //RD
            yield return new Point(rectangle.X, rectangle.Y + rectangle.Height); //LD
        }
    }
}
