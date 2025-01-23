using SixLabors.ImageSharp;

namespace TagsCloudVisualization.CloudLayouter;

public static class RectangleExtensions
{
    public static bool IntersectsWithAny(this RectangleF rectangle, IEnumerable<RectangleF> rectangles)
    {
        return rectangles.Any(existingRectangle => existingRectangle.IntersectsWith(rectangle));
    }
}