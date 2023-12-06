using System.Drawing;

namespace TagCloud;

public class CircularCloudLayouter : ICircularCloudLayouter
{
    private readonly Point center;
    private readonly SpiralGenerator spiral;
    public IList<Rectangle> Rectangles { get; }

    public CircularCloudLayouter(Point center)
    {
        this.center = center;
        Rectangles = new List<Rectangle>();
        spiral = new SpiralGenerator(center);
    }

    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        if (rectangleSize.Height <= 0 || rectangleSize.Width <= 0)
        {
            throw new ArgumentException("Rectangle size parameters should be positive");
        }

        var rectangle = new Rectangle(spiral.GetNextPoint(), rectangleSize);
        while (!CanPlaceRectangle(rectangle))
        {
            rectangle = new Rectangle(spiral.GetNextPoint() - rectangleSize / 2, rectangleSize);
        }

        var rectLocation = rectangle.Location;
        var deltaX = center.X - rectLocation.X;
        var deltaY = center.Y - rectLocation.Y;
        var directionToCenter = ((deltaX != 0) ? deltaX / Math.Abs(deltaX) : 0,
            (deltaY != 0) ? deltaY / Math.Abs(deltaY) : 0);

        ShiftRectangleInXDirection(ref rectangle, directionToCenter);
        ShiftRectangleInYDirection(ref rectangle, directionToCenter);

        Rectangles.Add(rectangle);

        return rectangle;
    }

    private void ShiftRectangleInXDirection(ref Rectangle rectangle, (int, int) vector)
    {
        while (CanPlaceRectangle(rectangle) && vector.Item1 != 0 && rectangle.X != center.X)
        {
            rectangle.Offset(vector.Item1, 0);
        }

        rectangle.Offset(-vector.Item1, 0);
    }

    private void ShiftRectangleInYDirection(ref Rectangle rectangle, (int, int) vector)
    {
        while (CanPlaceRectangle(rectangle) && vector.Item2 != 0 && rectangle.Y != center.Y)
        {
            rectangle.Offset(0, vector.Item2);
        }

        rectangle.Offset(0, -vector.Item2);
    }

    private bool CanPlaceRectangle(Rectangle newRectangle)
    {
        return !Rectangles.Any(rectangle => rectangle.IntersectsWith(newRectangle));
    }
}