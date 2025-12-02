using System.Drawing;
using static System.Math;

namespace TagsCloudVisualization;

public class CircularCloudLayouter(Point center)
{
    private const double radiusStep = 1;
    private const double angleStep = .01;
    private double radius = 0;
    private double angle = 0;
    private readonly List<Rectangle> placedRectangles = [];

    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
            throw new ArgumentException("Recatngle size should be greater then zero", nameof(rectangleSize));

        var rectangle = FindRectangleWithCorrectPosition(rectangleSize);
        placedRectangles.Add(rectangle);
        return rectangle;
    }

    private Rectangle FindRectangleWithCorrectPosition(Size size)
    {
        if (radius == 0)
        {
            radius += radiusStep;
            return new Rectangle(center - size / 2, size);
        }

        while (!CanPlaceRectangle(CreateRectangleAwayFromCenter(center, angle, radius, size)))
        {
            angle += angleStep;

            if (angle > 2 * PI)
            {
                angle = 0;
                radius += radiusStep;
            }
        }

        return PullRectangleToCenter(size);
    }

    private bool CanPlaceRectangle(Rectangle rectangle)
    {
        foreach (var placedRectangle in placedRectangles)
            if (rectangle.IntersectsWith(placedRectangle))
                return false;

        return true;
    }

    /// <summary>
    /// Pulls a rectangle toward the center of the cloud until it is centered (radius + circumscribingCircleRadius = 0) or intersects with another rectangle.
    /// </summary>
    private Rectangle PullRectangleToCenter(Size size)
    {
        var currentRadius = radius;
        var circumscribingCircleRadius = GetCircumscribingCircleRadius(size);

        while (currentRadius > -circumscribingCircleRadius
            && CanPlaceRectangle(CreateRectangleAwayFromCenter(center, angle, currentRadius, size)))
            currentRadius -= radiusStep;

        currentRadius += radiusStep;
        return CreateRectangleAwayFromCenter(center, angle, currentRadius, size);
    }

    /// <summary>
    /// The method creates a rectangle at a distance from the cloud center to the circumscribed circle of a rectangle.
    /// </summary>
    private static Rectangle CreateRectangleAwayFromCenter(Point center, double angle, double distance, Size size)
    {
        var circumscribingCircleRadius = GetCircumscribingCircleRadius(size);
        var location = CreatePointAwayFromCenter(center, angle, distance + circumscribingCircleRadius) - size / 2;

        return new Rectangle(location, size);
    }

    private static double GetCircumscribingCircleRadius(Size size)
        => Sqrt(
            (size.Width / 2) * (size.Width / 2)
            + (size.Height / 2) * (size.Height / 2));

    private static Point CreatePointAwayFromCenter(Point center, double angle, double distance)
        => new(
            center.X + (int)Round(distance * Cos(angle)),
            center.Y + (int)Round(distance * Sin(angle)));
}
