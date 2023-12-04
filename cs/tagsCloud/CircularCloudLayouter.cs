using System.Drawing;

namespace tagsCloud;

public class CircularCloudLayouter(Point center) : ICircularCloudLayouter
{
    public List<Rectangle> Rectangles { get; } = new();
    private readonly Spiral _spiral = new(center);
    private readonly Size _sizeZone = new(500, 500);
    private Rectangle _zonePossibleIntersections;
    private List<Rectangle> _nearestRectangles;

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
        var point = _spiral.GetPoint();
        var rect = new Rectangle(point, rectangleSize);
        GetNearestRectangles(point);
        while (_nearestRectangles.Any(x => x.IntersectsWith(rect)))
        {
            point = _spiral.GetPoint();
            rect = new Rectangle(point, rectangleSize);
            if (!Utils.IsInside(rect, _zonePossibleIntersections))
                GetNearestRectangles(point);
        }

        return rect;
    }

    private void GetNearestRectangles(Point point)
    {
        _zonePossibleIntersections =
            new Rectangle(new Point(point.X - _sizeZone.Width / 2, point.Y - _sizeZone.Height / 2), _sizeZone);
        _nearestRectangles = Rectangles.Where(x =>
            x.IntersectsWith(_zonePossibleIntersections) || Utils.IsInside(_zonePossibleIntersections, x)).ToList();
    }

    public List<Point> GetRectanglesLocation()
    {
        return Rectangles.Select(rectangle => rectangle.Location).ToList();
    }
}