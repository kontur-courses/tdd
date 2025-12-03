using System.Drawing;

namespace TagsCloudVisualization.Extensions;

public static class RectangleExtensions
{
    public static bool HasIntersections(this Rectangle rect, IEnumerable<Rectangle> rectangles) =>
        rectangles.Any(r => r.IntersectsWith(rect));
}