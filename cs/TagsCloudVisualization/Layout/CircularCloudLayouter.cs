using System.Drawing;

namespace TagsCloudVisualization.Layout;

public class CircularCloudLayouter(Point center, IPlacementPointSource placementPointSource, IRectangleCompactor compactor)
{
    private readonly List<Rectangle> rectangles = [];

    public IReadOnlyList<Rectangle> Rectangles => rectangles.AsReadOnly();

    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(rectangleSize.Width);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(rectangleSize.Height);

        foreach (var point in placementPointSource.GetPoints())
        {
            var rectangle = CreateCenteredAt(point, rectangleSize);

            if (rectangles.Exists(rect => rect.IntersectsWith(rectangle)))
                continue;

            rectangle = compactor.MoveTowardsCenter(rectangle, rectangles, center);
            rectangles.Add(rectangle);
            return rectangle;
        }

        throw new ArgumentOutOfRangeException($"No suitable placement point found for rectangle of size {rectangleSize}");
    }

    private static Rectangle CreateCenteredAt(Point center, Size size) =>
        new(center.X - size.Width / 2, center.Y - size.Height / 2, size.Width, size.Height);
}
