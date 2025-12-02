using System.Drawing;

namespace TagsCloudVisualization;

public class CircularCloudLayouter(Point center)
{
    private readonly List<Rectangle> rectangles = [];
    private readonly List<Point> freePointsSpiral = [];
    private ISpiral spiral = new ArchimedeanSpiral(center);
 
    public IReadOnlyList<Rectangle> Rectangles => rectangles;
    public Point Center => center;

    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        if (rectangleSize.Height <= 0 || rectangleSize.Width <= 0)
        {
            throw new ArgumentException("Received non-positive height or length, but expected positive");
        }
        
        var newRectangle = CreateRectangleAtSuitablePointOnSpiral(rectangleSize);
        var intersectingRectangle = rectangles.FirstOrDefault(x => x.IntersectsWith(newRectangle));
        
        if (rectangles.Count > 0)
        {
            while (intersectingRectangle.IsEmpty)
            {
                newRectangle = ShiftRectangleToCenter(newRectangle);
                intersectingRectangle = rectangles.FirstOrDefault(x => x.IntersectsWith(newRectangle));
            }
        }
        
        if (!intersectingRectangle.IsEmpty)
        {
            newRectangle = ShiftRectangleFromCenter(newRectangle);
        }

        rectangles.Add(newRectangle);
        
        return newRectangle;
    }
    
    public void SetNewSpiral(ISpiral newSpiral)
    {
        spiral = newSpiral;
    }

    private Rectangle CreateRectangleAtSuitablePointOnSpiral(Size rectangleSize)
    {
        var point = spiral.GetNextPoint();

        if (TryRectangleOnFreePointSpiral(rectangleSize, out var rectangle))
        {
            return rectangle;
        }
        
        while (rectangles.Any(x => x.Contains(point)))
        {
            point = spiral.GetNextPoint();
        }

        while (rectangles.Any(x => x.IsRectanglesIntersect(point, rectangleSize)))
        {
            freePointsSpiral.Add(point);
            point = spiral.GetNextPoint();
        }
        
        return CreateRectangle(point, rectangleSize);
    }

    private bool TryRectangleOnFreePointSpiral(Size rectangleSize, out Rectangle rectangle)
    {
        var occupiedPoints = new HashSet<Point>();
        
        foreach (var point in freePointsSpiral)
        {
            if (rectangles.Any(x => x.Contains(point)))
            {
                occupiedPoints.Add(point);
                continue;
            }

            if (rectangles.Any(x => x.IsRectanglesIntersect(point, rectangleSize)))
            {
                continue;
            }
            
            occupiedPoints.Add(point);
            freePointsSpiral.RemoveAll(x => occupiedPoints.Contains(x));
            rectangle = CreateRectangle(point, rectangleSize);
            return true;
        }

        rectangle = Rectangle.Empty;
        return false;
    }

    private Rectangle ShiftRectangleFromCenter(Rectangle rectangle)
    {
        var rectangleCenter = rectangle.GetCenter();
        var offsetX = Math.Sign(rectangleCenter.X - center.X);
        var offsetY = Math.Sign(rectangleCenter.Y - center.Y);
        
        rectangle.Offset(offsetX, offsetY);
        
        return rectangle;
    }

    private Rectangle ShiftRectangleToCenter(Rectangle rectangle)
    {
        var rectangleCenter = rectangle.GetCenter();
        var offsetX = Math.Sign(center.X - rectangleCenter.X);
        var offsetY = Math.Sign(center.Y - rectangleCenter.Y);
        
        rectangle.Offset(offsetX, offsetY);
        
        return rectangle;
    }

    private static Rectangle CreateRectangle(Point pointCenter, Size size)
    {
        return new Rectangle(pointCenter.X - size.Width / 2, pointCenter.Y - size.Height / 2, size.Width, size.Height);
    }
}