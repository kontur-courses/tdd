using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public static class RectangleExtension
    {
        public static IEnumerable<Point> GetCorners(this Rectangle rect)
        {
            yield return new Point(rect.Left, rect.Top);
            yield return new Point(rect.Right, rect.Top);
            yield return new Point(rect.Right, rect.Bottom);
            yield return new Point(rect.Left, rect.Bottom);
        }
    }
}