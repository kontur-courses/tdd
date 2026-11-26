using System.Drawing;

namespace TagsCloudVisualization;

public class CircularCloudLayouter
{
    private const double SpiralAngleStep = 0.075;
    private const double SpiralPitchFactor = 0.5;

    private readonly Point center;
    private readonly List<Rectangle> placedRectangles = new();

    public CircularCloudLayouter(Point center)
    {
        this.center = center;
    }

    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(rectangleSize),
                rectangleSize,
                "Rectangle dimensions must be positive.");
        }
        var rectangle = placedRectangles.Count == 0
            ? CreateCenteredRectangle(center, rectangleSize)
            : FindAvailableRectangle(rectangleSize);

        placedRectangles.Add(rectangle);

        return rectangle;
    }

    private static Rectangle CreateCenteredRectangle(Point center, Size size)
    {
        return new Rectangle(
            center.X - size.Width / 2,
            center.Y - size.Height / 2,
            size.Width,
            size.Height);
    }

    private bool OverlapsPlacedRectangle(Rectangle candidate)
    {
        foreach (var placedRectangle in placedRectangles)
        {
            if (placedRectangle.IntersectsWith(candidate))
                return true;
        }

        return false;
    }

    private Rectangle FindAvailableRectangle(Size rectangleSize)
    {
        var angle = 0.0;
        var spiralPitch = Math.Max(
            rectangleSize.Width,
            rectangleSize.Height) * SpiralPitchFactor;
        while (true)
        {
            angle += SpiralAngleStep;

            var radius = spiralPitch * angle / (2 * Math.PI);

            var candidateCenter = new Point(
                center.X + (int)Math.Round(radius * Math.Cos(angle)),
                center.Y + (int)Math.Round(radius * Math.Sin(angle)));

            var candidate = CreateCenteredRectangle(
                candidateCenter,
                rectangleSize);

            if (!OverlapsPlacedRectangle(candidate))
                return MoveTowardsCenter(candidate);
        }
    }

    private double GetDistanceSquared(Rectangle rectangle)
    {
        var deltaX = rectangle.X + rectangle.Width / 2.0 - center.X;
        var deltaY = rectangle.Y + rectangle.Height / 2.0 - center.Y;

        return deltaX * deltaX + deltaY * deltaY;
    }

    private Rectangle MoveTowardsCenter(Rectangle rectangle)
    {
        while (true)
        {
            var closestRectangle = rectangle;
            var closestDistance = GetDistanceSquared(rectangle);

            for (var offsetX = -1; offsetX <= 1; offsetX++)
            {
                for (var offsetY = -1; offsetY <= 1; offsetY++)
                {
                    if (offsetX == 0 && offsetY == 0)
                        continue;

                    var candidate = new Rectangle(
                        rectangle.X + offsetX,
                        rectangle.Y + offsetY,
                        rectangle.Width,
                        rectangle.Height);

                    var candidateDistance = GetDistanceSquared(candidate);

                    if (candidateDistance >= closestDistance ||
                        OverlapsPlacedRectangle(candidate))
                        continue;

                    closestRectangle = candidate;
                    closestDistance = candidateDistance;
                }
            }

            if (closestRectangle.Equals(rectangle))
                return rectangle;

            rectangle = closestRectangle;
        }
    }
}