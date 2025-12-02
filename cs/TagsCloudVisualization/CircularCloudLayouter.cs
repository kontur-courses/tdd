using System.Drawing;

namespace TagsCloudVisualization;

public class CircularCloudLayouter(Point center)
{
    private const double MinTargetStep = 1.0;
    private const double TargetStepFactor = 0.5;
    private const double AngleStepRadiusBias = 1.0;
    private const double MinAngleStep = 0.05;
    private const double MaxAngleStep = 0.8;

    private readonly List<Rectangle> _createdRectangles = [];

    private double _currentAngle;

    public IReadOnlyList<Rectangle> CreatedRectangles => _createdRectangles;

    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        if (rectangleSize.Width <= 0)
            throw new ArgumentException("Ширина должна быть положительным числом", nameof(rectangleSize));

        if (rectangleSize.Height <= 0)
            throw new ArgumentException("Высота должна быть положительным числом", nameof(rectangleSize));

        if (_createdRectangles.Count == 0)
        {
            var rectangle = CreateRectangle(rectangleSize, center.X, center.Y);
            _createdRectangles.Add(rectangle);
            return rectangle;
        }

        var candidate = GetFirstNonIntersectingRectangle(rectangleSize);
        var movedToCenter = MoveRectangleToCenter(candidate);

        _createdRectangles.Add(movedToCenter);
        return movedToCenter;
    }

    private Rectangle GetFirstNonIntersectingRectangle(Size rectangleSize)
    {
        while (true)
        {
            var pointOnSpiral = GetNextSpiralPoint(rectangleSize);
            var candidate = CreateRectangle(rectangleSize, pointOnSpiral.X, pointOnSpiral.Y);

            if (!_createdRectangles.Any(candidate.IntersectsWith))
                return candidate;
        }
    }

    private Rectangle MoveRectangleToCenter(Rectangle candidate)
    {
        var current = candidate;

        while (true)
        {
            var centerX = current.X + current.Width / 2;
            var centerY = current.Y + current.Height / 2;

            var stepX = Math.Sign(center.X - centerX);
            var stepY = Math.Sign(center.Y - centerY);

            if (stepX == 0 && stepY == 0)
                break;

            var movedCenterX = centerX + stepX;
            var movedCenterY = centerY + stepY;

            var moved = CreateRectangle(current.Size, movedCenterX, movedCenterY);

            if (_createdRectangles.Any(moved.IntersectsWith))
                break;

            current = moved;
        }

        return current;
    }

    private Point GetNextSpiralPoint(Size nextRectangleSize)
    {
        var minSide = Math.Min(nextRectangleSize.Width, nextRectangleSize.Height);

        var targetStep = Math.Max(MinTargetStep, minSide * TargetStepFactor);

        var angleStep = targetStep / (_currentAngle + AngleStepRadiusBias);
        angleStep = Math.Clamp(angleStep, MinAngleStep, MaxAngleStep);

        _currentAngle += angleStep;

        var radius = _currentAngle;

        var x = center.X + (int)Math.Round(radius * Math.Cos(_currentAngle));
        var y = center.Y + (int)Math.Round(radius * Math.Sin(_currentAngle));

        return new Point(x, y);
    }

    private static Rectangle CreateRectangle(Size size, int centerX, int centerY)
    {
        var left = centerX - size.Width / 2;
        var top = centerY - size.Height / 2;
        return new Rectangle(left, top, size.Width, size.Height);
    }
}
