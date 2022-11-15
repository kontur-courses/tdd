using System.Drawing;
using TagsCloudVisualization.Interfaces;

namespace TagsCloudVisualization;

public class CircularCloudLayouter : ICloudLayouter
{
    private readonly Point center;
    private readonly List<Rectangle> rectangles = new();
    private readonly ISpiralIterator iterator;

    public IReadOnlyCollection<Rectangle> Rectangles() => rectangles;

    public Point Center() => center;

    public CircularCloudLayouter(Point center)
    {
        this.center = center;
        iterator = new ArchimedeanSpiralIterator(new ArchimedeanSpiral(center));
    }

    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
        {
            throw new ArgumentException("Rectangle sizes should be positive", nameof(rectangleSize));
        }

        var rectangleLocation = CalculateRectangleLocation(iterator.Next(), rectangleSize);
        var rectangle = new Rectangle(rectangleLocation, rectangleSize);

        while (IntersectsWithAny(rectangle))
        {
            var rectangleCenter = iterator.Next();
            rectangle.Location = CalculateRectangleLocation(rectangleCenter, rectangleSize);
        }

        rectangles.Add(rectangle);
        return rectangle;
    }

    private static Point CalculateRectangleLocation(Point rectangleCenter, Size rectangleSize)
    {
        var rectangleTop = rectangleCenter.Y - rectangleSize.Height / 2;
        var rectangleLeft = rectangleCenter.X - rectangleSize.Width / 2;
        return new Point(rectangleLeft, rectangleTop);
    }

    private bool IntersectsWithAny(Rectangle rectangle)
    {
        return rectangles.Any(curr => curr.IntersectsWith(rectangle));
    }
}