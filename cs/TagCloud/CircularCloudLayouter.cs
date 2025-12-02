using SixLabors.ImageSharp;
using TagCloud.CoordinatesProviders;
using TagCloud.Utils;

namespace TagCloud;

public class CircularCloudLayouter(Point center, ICoordinatesProvider coordinatesProvider)
{
    public readonly List<Rectangle> Rectangles = [];
    
    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        if (CheckSizeIncorrectness(rectangleSize))
            throw new ArgumentException("Size is incorrect");
        
        var rect = new Rectangle(GetNextRectanglePoint(rectangleSize), rectangleSize);
        var shiftedRect = RectangleUtils.ShiftToCenter(rect, center, Rectangles);
        Rectangles.Add(shiftedRect);
        return shiftedRect;
    }

    private bool CheckSizeIncorrectness(Size size) =>
        size.Width <= 0 || size.Height <= 0;
    
    private Point GetNextRectanglePoint(Size rectangleSize)
    {
        foreach (var point in coordinatesProvider.GetPoints().Select(Point.Round))
        {
            var possibleValidPoint = new Point(point.X - rectangleSize.Width / 2, point.Y - rectangleSize.Height / 2);
            var possibleValidRectangle = new Rectangle(possibleValidPoint, rectangleSize);
            var isIntersects = Rectangles.Any(existingRectangle => possibleValidRectangle.IntersectsWith(existingRectangle));

            if (!isIntersects)
                return possibleValidPoint;
        }

        throw new Exception($"Can't find valid point for next rectangle with width: {rectangleSize.Width} and height: {rectangleSize.Height}");
    }
}