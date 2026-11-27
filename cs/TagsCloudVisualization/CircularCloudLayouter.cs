using System.Diagnostics;
using System.Drawing;

namespace TagsCloudVisualization;

public class CircularCloudLayouter(Point center)
{
    private readonly ArchimedeanSpiral spiral = new(center);
    private readonly RectangleCompactor compactor = new(center);
    private readonly List<Rectangle> rectangles = [];

    public IReadOnlyList<Rectangle> Rectangles => rectangles.AsReadOnly();

    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(rectangleSize.Width);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(rectangleSize.Height);

        foreach (var point in spiral.GetPoints())
        {
            var rectangle = CreateCenteredAt(point, rectangleSize);

            if (rectangles.Exists(rect => rect.IntersectsWith(rectangle)))
                continue;

            rectangle = compactor.MoveTowardsCenter(rectangle, rectangles);
            rectangles.Add(rectangle);
            return rectangle;
        }

        throw new UnreachableException("The spiral is infinite, so a free place is always found.");
    }

    private static Rectangle CreateCenteredAt(Point center, Size size) =>
        new(center.X - size.Width / 2, center.Y - size.Height / 2, size.Width, size.Height);
}
