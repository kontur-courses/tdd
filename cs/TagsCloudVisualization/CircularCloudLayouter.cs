using System.Drawing;

namespace TagsCloudVisualization;

public class CircularCloudLayouter : ICloudLayouter
{
    private readonly Point _center;
    private readonly List<Rectangle> _rectangles = new();
    private readonly ISpiralIterator _iterator;

    public ICollection<Rectangle> Rectangles() => _rectangles;

    public Point Center() => _center;

    public CircularCloudLayouter(Point center)
    {
        _center = center;
        _iterator = new ArchimedeanSpiralIterator(new ArchimedeanSpiral(center));
    }

    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
        {
            throw new ArgumentException("Rectangle sizes should be positive", nameof(rectangleSize));
        }
        
        var rectangleLocation = CalculateRectangleLocation(_iterator.Next(), rectangleSize);
        var rectangle = new Rectangle(rectangleLocation, rectangleSize);
        
        while (IntersectsWithAny(rectangle))
        {
            var rectangleCenter = _iterator.Next();
            rectangle.Location = CalculateRectangleLocation(rectangleCenter, rectangleSize);
        }
        
        _rectangles.Add(rectangle);
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
        return _rectangles.Any(curr => curr.IntersectsWith(rectangle));
    }
}