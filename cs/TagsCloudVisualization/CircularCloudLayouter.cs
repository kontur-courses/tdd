using System.Diagnostics;
using System.Drawing;

namespace TagsCloudVisualization;

public class CircularCloudLayouter(Point center, double spiralStep = 5)
{
    private readonly ArchimedeanSpiral spiral = new(center, spiralStep);
    private readonly List<Rectangle> rectangles = [];

    public IReadOnlyList<Rectangle> Rectangles => rectangles.AsReadOnly();

    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(rectangleSize.Width);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(rectangleSize.Height);

        foreach (var point in spiral.GetSpiralPoints())
        {
            var location = new Point(point.X - rectangleSize.Width / 2,
                point.Y - rectangleSize.Height / 2);

            var rectangle = new Rectangle(location, rectangleSize);

            if (rectangles.Exists(placed => placed.IntersectsWith(rectangle)))
                continue;

            rectangles.Add(rectangle);
            return rectangle;
        }

        throw new UnreachableException("The spiral is infinite, so a free place is always found.");
    }
}
