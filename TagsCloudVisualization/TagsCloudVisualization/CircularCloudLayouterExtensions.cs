using System.Drawing;

namespace TagsCloudVisualization;

public static class CircularCloudLayouterExtensions
{
    public static IEnumerable<Rectangle> GenerateLayout(this ICircularCloudLayouter layouter, Size[] sizes)
    {
        return sizes.Select(layouter.PutNextRectangle);
    }
}