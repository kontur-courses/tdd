using System.Drawing;
using TagsCloudVisualization.Abstractions;

namespace TagsCloudVisualization;

public class CircularCloudLayouter : ICloudLayouter
{
    private readonly List<Rectangle> rectangles = new();
    private readonly IPointGenerator pointGenerator;

    public CircularCloudLayouter(Point center)
    {
        pointGenerator = new SpiralPointGenerator(center);
    }

    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        if (rectangleSize.Height <= 0)
            throw new ArgumentException("Heigth should be positive");
        if (rectangleSize.Width <= 0)
            throw new ArgumentException("Width should be positive");

        Rectangle rectangle;
        while (true)
        {
            var point = pointGenerator.Next();
            rectangle = new Rectangle(point, rectangleSize);
            if (CheckPosition(rectangle))
                break;
        }

        rectangles.Add(rectangle);
        return rectangle;
    }

    private bool CheckPosition(Rectangle newRectangle)
    {
        return !rectangles.Any(x => x.IntersectsWith(newRectangle));
    }
}