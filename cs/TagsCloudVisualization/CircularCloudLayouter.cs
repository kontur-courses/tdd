using System.Drawing;

namespace TagsCloudVisualization;

public class CircularCloudLayouter(Point center) : ICircularCloudLayouter
{
    private const double AngleStepInRadians = 0.05;
    private const double RadiusStep = 0.5;

    private readonly List<Rectangle> placedRectangles = [];

    public Point Center { get; } = center;
    public IReadOnlyList<Rectangle> PlacedRectangles => placedRectangles;

    public CircularCloudLayouter() : this(Point.Empty)
    {
    }

    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        if (rectangleSize.Width <= 0)
            throw new ArgumentException("Rectangle width must be positive.", nameof(rectangleSize));
        if (rectangleSize.Height <= 0)
            throw new ArgumentException("Rectangle height must be positive.", nameof(rectangleSize));

        var rectangle = FindPositionFor(rectangleSize);
        rectangle = CompactRectangle(rectangle);

        placedRectangles.Add(rectangle);
        return rectangle;
    }

    private Rectangle FindPositionFor(Size rectangleSize)
    {
        return GetRectanglePositions().Select(point => CreateRectangleCenteredAt(point, rectangleSize))
                                      .First(rect => !IntersectsWithAny(rect));
    }

    private Rectangle CompactRectangle(Rectangle rectangle)
    {
        while (true)
        {
            var currentCenter = GetCenter(rectangle);

            var dx = Math.Sign(Center.X - currentCenter.X);
            var dy = Math.Sign(Center.Y - currentCenter.Y);

            var movedX = TryMove(ref rectangle, dx, 0);
            var movedY = TryMove(ref rectangle, 0, dy);

            if (!movedX && !movedY)
                return rectangle;
        }
    }

    private bool TryMove(ref Rectangle rectangle, int dx, int dy)
    {
        if (dx == 0 && dy == 0)
            return false;

        var movedRectangle = new Rectangle(rectangle.X + dx, rectangle.Y + dy,
                                      rectangle.Width, rectangle.Height);

        if (IntersectsWithAny(movedRectangle))
            return false;

        rectangle = movedRectangle;

        return true;
    }

    private IEnumerable<Point> GetRectanglePositions()
    {
        yield return Center;

        var angle = 0.0;
        var previousPoint = Center;

        while (true)
        {
            angle += AngleStepInRadians;

            var point = GetSpiralPoint(angle);
            if (point == previousPoint)
                continue;

            previousPoint = point;

            yield return point;
        }
    }

    private Point GetSpiralPoint(double angle)
    {
        var radius = RadiusStep * angle;
        var (sin, cos) = Math.SinCos(angle);

        return new(Center.X + (int)Math.Round(radius * cos),
                   Center.Y + (int)Math.Round(radius * sin));
    }

    private static Rectangle CreateRectangleCenteredAt(Point center, Size size) =>
        new(center.X - size.Width / 2, center.Y - size.Height / 2,
            size.Width, size.Height);

    private bool IntersectsWithAny(Rectangle rectangle) =>
        placedRectangles.Any(placedRectangle => placedRectangle.IntersectsWith(rectangle));

    private static Point GetCenter(Rectangle rectangle) =>
        new(rectangle.X + rectangle.Width / 2, rectangle.Y + rectangle.Height / 2);
}
