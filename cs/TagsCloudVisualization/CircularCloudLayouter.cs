using System.Drawing;

namespace TagsCloudVisualization;

public class CircularCloudLayouter : ICircularCloudLayouter
{
    private const double SpiralStep = 0.5;
    private const double AngleStep = 0.1;

    private readonly Point center;
    private readonly List<Rectangle> rectangles = new();
    private readonly RectangleSpatialGrid spatialGrid = new();

    private double maxCornerDistance;

    public IReadOnlyList<Rectangle> Rectangles => rectangles;

    public CircularCloudLayouter(Point center)
    {
        this.center = center;
    }

    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
            throw new ArgumentOutOfRangeException("Size must be positive", nameof(rectangleSize));

        if (rectangles.Count == 0)
        {
            var firstRect = CreateRectangle(center, rectangleSize);
            AddRectangle(firstRect);
            return firstRect;
        }

        var candidate = FindPositionOnSpiral(rectangleSize);
        var compacted = CompactToCenter(candidate);

        AddRectangle(compacted);
        return compacted;
    }

    private Rectangle FindPositionOnSpiral(Size size)
    {
        var maxRadius = CalculateMaxSearchRadius(size);

        double angle = 0;
        double radius = 0;

        while (radius <= maxRadius)
        {
            angle += AngleStep;
            radius += SpiralStep;

            var x = center.X + (int)Math.Round(radius * Math.Cos(angle));
            var y = center.Y + (int)Math.Round(radius * Math.Sin(angle));

            var candidate = CreateRectangle(new Point(x, y), size);

            if (!spatialGrid.IntersectsWithAny(candidate))
                return candidate;
        }
        throw new InvalidOperationException("Unable to find a place for rectangle");
    }

    private Rectangle CompactToCenter(Rectangle rectangle)
    {
        var current = rectangle;

        var dirX = Math.Sign(center.X - (current.X + current.Width / 2));
        var dirY = Math.Sign(center.Y - (current.Y + current.Height / 2));

        current = MoveAlongX(current, dirX);
        current = MoveAlongY(current, dirY);
        current = MoveAlongX(current, dirX);

        return current;
    }

    private Rectangle MoveAlongX(Rectangle current, int dir)
    {
        if (dir == 0) return current;

        while (true)
        {
            var next = new Rectangle(current.X + dir, current.Y, current.Width, current.Height);

            var nextCenterX = next.X + next.Width / 2;
            if (dir > 0 && nextCenterX > center.X)
                return current;
            if (dir < 0 && nextCenterX < center.X)
                return current;

            if (spatialGrid.IntersectsWithAny(next))
                return current;

            current = next;
        }
    }

    private Rectangle MoveAlongY(Rectangle current, int dir)
    {
        if (dir == 0) return current;

        while (true)
        {
            var next = new Rectangle(current.X, current.Y + dir,current.Width, current.Height);

            var nextCenterY = next.Y + next.Height / 2;
            if (dir > 0 && nextCenterY > center.Y)
                return current;
            if (dir < 0 && nextCenterY < center.Y)
                return current;
            if (spatialGrid.IntersectsWithAny(next))
                return current;

            current = next;
        }
    }

    private double CalculateMaxSearchRadius(Size rectangleSize)
    {
        var newRectangleRadius = Math.Sqrt(
            (double)rectangleSize.Width * rectangleSize.Width +
            (double)rectangleSize.Height * rectangleSize.Height) / 2;

        return maxCornerDistance + newRectangleRadius + SpiralStep;
    }

    private void AddRectangle(Rectangle rectangle)
    {
        rectangles.Add(rectangle);
        spatialGrid.Add(rectangle);
        var cornerDistance = GetCornerDistances(rectangle).Max();
        if (cornerDistance > maxCornerDistance)
            maxCornerDistance = cornerDistance;
    }

    private IEnumerable<double> GetCornerDistances(Rectangle rectangle)
    {
        yield return Distance(center, new Point(rectangle.Left, rectangle.Top));
        yield return Distance(center, new Point(rectangle.Right, rectangle.Top));
        yield return Distance(center, new Point(rectangle.Right, rectangle.Bottom));
        yield return Distance(center, new Point(rectangle.Left, rectangle.Bottom));
    }

    private static Rectangle CreateRectangle(Point centerPoint, Size size)
    {
        return new Rectangle(
            centerPoint.X - size.Width / 2,
            centerPoint.Y - size.Height / 2,
            size.Width,
            size.Height);
    }

    private static double Distance(Point first, Point second)
    {
        var dx = (double)first.X - second.X;
        var dy = (double)first.Y - second.Y;
        return Math.Sqrt(dx * dx + dy * dy);
    }
}