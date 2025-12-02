using System.Drawing;

namespace TagCloud;

public class CircularCloudLayouter(Point center)
{
    private readonly Spiral spiral = new(center);
    private readonly List<Rectangle> rectangles = [];
    private readonly List<(Point Center, double Radius)> circleBounds = [];

    public IReadOnlyCollection<Rectangle> Rectangles => rectangles.AsReadOnly();
    
    private const int MaxMoveStep = 64;

    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        if (rectangleSize.Width <= 0)
            throw new ArgumentOutOfRangeException(nameof(rectangleSize.Width),
                "Ширина прямоугольника должна быть положительной");

        if (rectangleSize.Height <= 0)
            throw new ArgumentOutOfRangeException(nameof(rectangleSize.Height),
                "Высота прямоугольника должна быть положительной");
        
        if (rectangles.Count == 0)
        {
            var first = CreateRectangleByCenter(center, rectangleSize);
            AddRectangle(first);
            return first;
        }
        
        var pointOnSpiral = spiral.GetNextPoint();
        var candidate = CreateRectangleByCenter(pointOnSpiral, rectangleSize);
        
        while (IntersectsWithAny(candidate))
        {
            pointOnSpiral = spiral.RetryGetNextPoint();
            candidate = CreateRectangleByCenter(pointOnSpiral, rectangleSize);
        }
        
        var compressed = MoveToCenter(candidate);
        AddRectangle(compressed);
        return compressed;
    }
    
    private void AddRectangle(Rectangle rect)
    {
        rectangles.Add(rect);
        var currCenter = GetCenter(rect);
        var currRadius = GetBoundingCircleRadius(rect);
        circleBounds.Add((currCenter, currRadius));
    }

    private static Rectangle CreateRectangleByCenter(Point point, Size size)
    {
        var x = point.X - size.Width / 2;
        var y = point.Y - size.Height / 2;
        return new Rectangle(x, y, size.Width, size.Height);
    }
    
    private bool IntersectsWithAny(Rectangle rect)
    {
        if (rectangles.Count == 0)
            return false;

        var candidateCenter = GetCenter(rect);
        var candidateRadius = GetBoundingCircleRadius(rect);

        for (var i = 0; i < rectangles.Count; i++)
        {
            var (centerExisting, radiusExisting) = circleBounds[i];
            
            if (!CirclesIntersect(candidateCenter, candidateRadius, centerExisting, radiusExisting))
                continue;
            
            if (rectangles[i].IntersectsWith(rect))
                return true;
        }

        return false;
    }

    private Rectangle MoveToCenter(Rectangle rectangle)
    {
        while (true)
        {
            var rectCenter = GetCenter(rectangle);
            var dx = Math.Sign(center.X - rectCenter.X);
            var dy = Math.Sign(center.Y - rectCenter.Y);

            if (dx == 0 && dy == 0)
                return rectangle;
            
            var movedX = MoveAlongAxis(rectangle, dx, 0);
            var movedXy = MoveAlongAxis(movedX, 0, dy);

            if (movedXy == rectangle)
                return movedXy;

            rectangle = movedXy;
        }
    }
    
    private Rectangle MoveAlongAxis(Rectangle rectangle, int dx, int dy)
    {
        if (dx == 0 && dy == 0)
            return rectangle;

        return FindBestPositionWithExponentialStep(rectangle, dx, dy);
    }

    private Rectangle FindBestPositionWithExponentialStep(Rectangle rectangle, int dx, int dy)
    {
        var best = rectangle;
        var bestCenter = GetCenter(best);

        var bestDistX = DistanceXToCenter(bestCenter);
        var bestDistY = DistanceYToCenter(bestCenter);

        var step = 1;
        while (step <= MaxMoveStep)
        {
            var shifted = rectangle with
            {
                X = rectangle.X + dx * step,
                Y = rectangle.Y + dy * step
            };

            var shiftedCenter = GetCenter(shifted);
            var distX = DistanceXToCenter(shiftedCenter);
            var distY = DistanceYToCenter(shiftedCenter);
            
            if ((dx != 0 && distX >= bestDistX) ||
                (dy != 0 && distY >= bestDistY))
                break;

            if (IntersectsWithAny(shifted))
                break;

            best = shifted;
            bestDistX = distX;
            bestDistY = distY;
            step *= 2;
        }

        return best;
    }
    
    private static Point GetCenter(Rectangle rect) =>
        new(rect.X + rect.Width / 2, rect.Y + rect.Height / 2);
    
    private static double GetBoundingCircleRadius(Rectangle rect)
    {
        var halfW = rect.Width / 2.0;
        var halfH = rect.Height / 2.0;
        return Math.Sqrt(halfW * halfW + halfH * halfH);
    }

    private int DistanceXToCenter(Point p) => Math.Abs(center.X - p.X);
    private int DistanceYToCenter(Point p) => Math.Abs(center.Y - p.Y);

    private static double DistanceSquared(Point p1, Point p2)
    {
        var dx = p1.X - p2.X;
        var dy = p1.Y - p2.Y;
        return (double)dx * dx + (double)dy * dy;
    }
    
    private static bool CirclesIntersect(Point c1, double r1, Point c2, double r2)
    {
        var distSq = DistanceSquared(c1, c2);
        var sum = r1 + r2;
        return distSq <= sum * sum;
    }
}
