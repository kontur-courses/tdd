using System.Drawing;
using TagsCloudLib.Abstractions.Visualizer;
using TagsCloudLib.Models;

namespace TagsCloudLib.Implementations.Visualizer;

public class BoundingBoxCalculator : IBoundingBoxCalculator
{
    public LayoutBounds Calculate(IEnumerable<Rectangle> rectangles)
    {
        var rects = rectangles.ToList();
        return new LayoutBounds(rects.Min(r => r.Left), rects.Min(r => r.Top), rects.Max(r => r.Right),
            rects.Max(r => r.Bottom)
        );
    }
}