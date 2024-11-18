using System.Drawing;

namespace TagsCloudVisualization;

public class CircularCloudLayouter
{
    private ArchimedeanSpiral spiral;
    private Point centerСloud;

    private List<Rectangle> rectangles;

    public Point CenterCloud => centerСloud;

    public List<Rectangle> GetRectangles => rectangles;

    public CircularCloudLayouter(Point center)
    {
        this.centerСloud = center;
        this.spiral = new ArchimedeanSpiral(center);
        this.rectangles = new List<Rectangle>();
    }

    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        if (rectangleSize.IsEmpty)
        {
            throw new ArgumentNullException("rectangle is empty");
        }
        if (rectangleSize.Height <= 0 || rectangleSize.Width <= 0)
        {
            throw new ArgumentOutOfRangeException("side less or equal zero");
        }
        Rectangle tempRectangle;
        do
        {
            Point nextPoint = spiral.GetNextPoint();
            tempRectangle = new Rectangle(new Point(nextPoint.X, nextPoint.Y), rectangleSize);
        }
        while (IsRectangleIntersect(tempRectangle));
        if (rectangles.Count > 1)
            tempRectangle = TryToMoveRectangleNearToCenter(tempRectangle);
        rectangles.Add(tempRectangle);
        return tempRectangle;
    }

    private Rectangle TryToMoveRectangleNearToCenter(Rectangle rectangle)
    {
        while (true)
        {
            var tempRectangle = rectangle;
            if (rectangle.X != centerСloud.X)
            {
                var directionX = rectangle.X < centerСloud.X ? 1 : -1;
                rectangle = MovingRectangleIfPossible(rectangle, true, directionX);
            }
            if (rectangle.Y != centerСloud.Y)
            {
                var directionY = rectangle.Y < centerСloud.Y ? 1 : -1;
                rectangle = MovingRectangleIfPossible(rectangle, false, directionY);
            }

            if (tempRectangle.Equals(rectangle))
                break;
        }
        return rectangle;
    }

    private Rectangle MovingRectangleIfPossible(Rectangle rectangle, bool isX, int direction)
    {
        var shiftPoint = isX ? new Point(direction, 0) : new Point(0, direction);
        var movedRectangle = rectangle with
        {
            Location = new Point(rectangle.X + shiftPoint.X, rectangle.Y + shiftPoint.Y)
        };

        if (!IsRectangleIntersect(movedRectangle))
        {
            rectangle = movedRectangle;
        }
        return rectangle;
    }

    private bool IsRectangleIntersect(Rectangle rectangleChecked) =>
    rectangles.Any(rectangleChecked.IntersectsWith);
}