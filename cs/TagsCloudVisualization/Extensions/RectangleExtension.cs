using System.Drawing;
using TagsCloudVisualization.Layouter;

namespace TagsCloudVisualization.Extensions;

public static class RectangleExtension
{
    public static IEnumerable<Point> GetPoints(this Rectangle rectangle)
    {
        if (rectangle.Size.Width <= 0 || rectangle.Size.Height <= 0)
            throw new IncorrectSizeException();
        return new[]
        {
            new Point(rectangle.Left, rectangle.Bottom), new Point(rectangle.Left, rectangle.Top),
            new Point(rectangle.Right, rectangle.Bottom), new Point(rectangle.Right, rectangle.Top)
        };
    }
}