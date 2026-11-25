using System.Drawing;

namespace TagsCloudVisualization;

public class CircularCloudLayouter
{
    private const double AngularStep = 0.1;

    private readonly Point center;
    private readonly List<Rectangle> placedRectangles = new();

    public CircularCloudLayouter(Point center)
    {
        this.center = center;
    }

    public Rectangle PutNextRectangle(Size rectangleSize)
    {
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
            rectangleSize.Height);

        while (true)
        {
            angle += AngularStep;

            var radius = spiralPitch * angle / (2 * Math.PI);

            var candidateCenter = new Point(
                center.X + (int)Math.Round(radius * Math.Cos(angle)),
                center.Y + (int)Math.Round(radius * Math.Sin(angle)));

            var candidate = CreateCenteredRectangle(
                candidateCenter,
                rectangleSize);

            if (!OverlapsPlacedRectangle(candidate))
                return candidate;
        }
    }
}