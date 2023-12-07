using System.Drawing;

namespace TagsCloudVisualization;

public static class CloudLayouterExtensions
{
    public static Rectangle GetBorders(this List<Rectangle> placedRectangles)
    {
        if (placedRectangles is null || placedRectangles.Count == 0)
            throw new InvalidOperationException("The list of placed rectangles cannot be null or empty");

        var left = placedRectangles.Min(r => r.Left);
        var right = placedRectangles.Max(r => r.Right);
        var top = placedRectangles.Min(r => r.Top);
        var bottom = placedRectangles.Max(r => r.Bottom);

        var width = right - left;
        var height = bottom - top;
        return new Rectangle(left, top, width, height);
    }

    public static bool AnyIntersectsWith(this IEnumerable<Rectangle> rectangles, Rectangle rectangle)
    {
        return rectangles
            .Any(r => r
                .IntersectsWith(rectangle));
    }
}