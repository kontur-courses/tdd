using System.Drawing;

namespace TagsCloudVisualization;

public class CircularCloudLayouter : ICircularCloudLayouter
{
    public Point Center { get; }

    private readonly List<Rectangle> _putRectangles = new();
    private readonly ArchimedeanSpiral _spiral = new(1);

    public CircularCloudLayouter(Point center)
    {
        Center = center;
    }

    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
            throw new ArgumentException("Rectangle size must be positive.", nameof(rectangleSize));

        Rectangle result;
        if (_putRectangles.Count == 0)
        {
            result = CreateCenteredRectangle(Center, rectangleSize);
        }
        else
        {
            result = GetNextPosition(deltaPhi: 0.087, rectangleSize);
            result = MoveToCenterWhilePossible(result, Center);
        }
        _putRectangles.Add(result);
        return result;
    }

    private Rectangle MoveToCenterWhilePossible(Rectangle rectangle, Point center)
    {
        while (true)
        {
            var rectCenter = rectangle.GetCenter();

            var dx = Math.Sign(center.X - rectCenter.X);
            var dy = Math.Sign(center.Y - rectCenter.Y);

            if (dx == 0 && dy == 0)
                return rectangle;

            if (TryShift(rectangle, dx, dy, out var shifted) ||
                TryShift(rectangle, dx, 0, out shifted) ||
                TryShift(rectangle, 0, dy, out shifted))
            {
                rectangle = shifted;
                continue;
            }

            return rectangle;
        }
    }

    private bool TryShift(Rectangle rectangle, int dx, int dy, out Rectangle shifted)
    {
        shifted = rectangle;

        if (dx == 0 && dy == 0)
            return false;

        shifted.Offset(dx, dy);
        return !HasAnyIntersections(shifted);
    }

    private Rectangle GetNextPosition(double deltaPhi, Size size)
    {
        double phi = 0;
        while (true)
        {
            var rho = _spiral.GetRho(phi);
            var x = Center.X + (int)Math.Round(ArchimedeanSpiral.GetX(rho, phi));
            var y = Center.Y + (int)Math.Round(ArchimedeanSpiral.GetY(rho, phi));

            var candidate = CreateCenteredRectangle(new(x, y), size);

            if (!HasAnyIntersections(candidate))
                return candidate;

            phi += deltaPhi;
        }
    }

    private static Rectangle CreateCenteredRectangle(Point center, Size size)
    {
        return new Rectangle(
            center.X - size.Width / 2,
            center.Y - size.Height / 2,
            size.Width,
            size.Height);
    }

    private bool HasAnyIntersections(Rectangle rectangle) => _putRectangles.Any(r => r.IntersectsWith(rectangle));
}
