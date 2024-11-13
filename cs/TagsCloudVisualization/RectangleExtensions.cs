using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization;

public static class RectangleExtensions
{
    public static bool IsIntersectsWithAny(this IEnumerable<Rectangle> rectangles, Rectangle rectangle)
    {
        return rectangles.Any(tag => tag.IntersectsWith(rectangle));
    }
}
