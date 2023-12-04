using System.Drawing;

namespace tagsCloud;

public class CircularCloudLayouter(Point center) : ICircularCloudLayouter
{
    public List<Rectangle> Rectangles { get; } = new();

    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        if (rectangleSize.Height <= 0 || rectangleSize.Width <= 0)
            throw new ArgumentException("Sides of the rectangle should not be non-positive");
        var rect = CreateNextRectangle(rectangleSize);
        Rectangles.Add(rect);
        return rect;
    }

    private Rectangle CreateNextRectangle(Size rectangleSize)
    {
        var spiral = new Spiral(center);
        var point = spiral.GetPoint();
        var rect = new Rectangle(point, rectangleSize);
        while (Rectangles.Any(x => x.IntersectsWith(rect)))
        {
            point = spiral.GetPoint();
            rect = new Rectangle(point, rectangleSize);
        }

        return rect;
    }

    public List<Point> GetRectanglesLocation()
    {
        return Rectangles.Select(rectangle => rectangle.Location).ToList();
    }
}