using System.Numerics;

namespace TagsCloudVisualization;

public class CircularCloudLayouter
{
    private readonly Point _center;
    private SortedList<float, Point> _nearestToTheCenterPoints;
    private List<Rectangle> _puttedRectangles;
    private enum _rotateRectangleDirections
    {
        none, bottom, left
    };

    public int Hash
    {
        get => _puttedRectangles.GetHashCode();
    }
    
    public CircularCloudLayouter(Point center)
    {
        if (center.IsEmpty)
            throw new ArgumentException("Center can't be empty");
        if (center.X < 0 || center.Y < 0)
            throw new ArgumentException("Center can't be located outside of drawing field");
            
        _center = center;
        _nearestToTheCenterPoints = new SortedList<float, Point>();
        _puttedRectangles = new List<Rectangle>();
        
        AddFreePoint(center);
    }
    
    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        
        if (rectangleSize.IsEmpty)
            throw new ArgumentException("Rectangle size can't be empty");
        if (rectangleSize.Width < 0 || rectangleSize.Height < 0) 
            throw new ArgumentException("Rectangle size can't have negative side value");

        var nearestFreePoint = GetNearestInsertionPoint(rectangleSize);
        var rectangle = new Rectangle(nearestFreePoint, rectangleSize);
        
        _puttedRectangles.Add(rectangle);
        AddVerticesToFreePoints(rectangle);

        return rectangle;
    }
    
    private Point GetNearestInsertionPoint(Size rectangleSize)
    {
        foreach (var nearestPoint in _nearestToTheCenterPoints.Values)
        {
            foreach (var rotateDirection in Enum.GetValues(typeof(_rotateRectangleDirections)))
            {
                var insertionPoint = GetInsertionPoint(rectangleSize, nearestPoint, rotateDirection);
                var rectangle = new Rectangle(insertionPoint, rectangleSize);

                if (!DoesItIntersect(rectangle)) return insertionPoint;
            }
        }
        
        throw new Exception("Can't put this rectangle");
    }

    private bool DoesItIntersect(Rectangle rectangle)
    {
        foreach (var puttedRectangle in _puttedRectangles)
            if (puttedRectangle.IntersectsWith(rectangle)) return true;
        return false;
    }

    private Point GetInsertionPoint(Size rectangleSize, Point point, object rotateDirection)
    { 
        var insertionPoint = new Point(point.X, point.Y);
        
        if (rotateDirection.Equals(_rotateRectangleDirections.bottom))
            insertionPoint.X -= rectangleSize.Width;
        if (rotateDirection.Equals(_rotateRectangleDirections.left))
            insertionPoint.Y -= rectangleSize.Height;

        return insertionPoint;
    }

    private void AddVerticesToFreePoints(Rectangle rectangle)
    {
        for (var i = 0; i <= rectangle.Width; i += rectangle.Width)
        for (var j = 0; j <= rectangle.Height; j += rectangle.Height) 
            AddFreePoint(new Point(rectangle.X + i, rectangle.Y + j));
    }

    private void AddFreePoint(Point point)
    {
        var distanceFromCenter = CountDistanceFromCenter(point);
        if (!_nearestToTheCenterPoints.ContainsKey(distanceFromCenter))
            _nearestToTheCenterPoints.Add(distanceFromCenter, point);
    }
    
    private float CountDistanceFromCenter(Point point)
    {
        var distanceFromCenter = new Vector2(point.X - _center.X, point.Y - _center.Y);
        return distanceFromCenter.Length();
    }
}