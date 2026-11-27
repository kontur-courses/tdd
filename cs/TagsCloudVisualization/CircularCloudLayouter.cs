using System.Diagnostics;
using System.Drawing;

namespace TagsCloudVisualization;

public class CircularCloudLayouter(Point center, double spiralStep = 5, double pointSpacing = 2)
{
    private readonly ArchimedeanSpiral spiral = new(center, spiralStep, pointSpacing);
    private readonly List<Rectangle> rectangles = [];

    public IReadOnlyList<Rectangle> Rectangles => rectangles.AsReadOnly();

    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(rectangleSize.Width);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(rectangleSize.Height);

        foreach (var point in spiral.GetPoints())
        {
            var location = new Point(point.X - rectangleSize.Width / 2,
                point.Y - rectangleSize.Height / 2);

            var rectangle = new Rectangle(location, rectangleSize);

            if (rectangles.Any(rect => rect.IntersectsWith(rectangle)))
                continue;

            rectangles.Add(rectangle);
            return rectangle;
        }

        throw new UnreachableException("The spiral is infinite, so a free place is always found.");
    }
}
