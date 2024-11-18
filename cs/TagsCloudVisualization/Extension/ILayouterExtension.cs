using System.Drawing;
using TagsCloudVisualization.CloudLayouter;

namespace TagsCloudVisualization.Extension;

public static class ILayouterExtension
{
    public static void PutRectangles(this ILayouter layouter, IEnumerable<Rectangle> rectangles)
    {
        foreach (var rectangle in rectangles)
        {
            layouter.PutNextRectangle(rectangle.Size);
        }
    }
}