using System.Drawing;

namespace TagsCloudVisualization;

public class CircularCloudLayouter : ICircularCloudLayouter
{
    private readonly IList<Rectangle> rectangles;
    private readonly Point center;
    private int layer;
    private double angle;
    private const double fullCircleTurn = Math.PI * 2;
    private const int distanceLayersDifference = 1;
    private const double betweenAngleDifference = Math.PI / 36; // 15°

    public CircularCloudLayouter(Point center)
    {
        this.center = center;
        rectangles = new List<Rectangle>();
        layer = 0;
        angle = 0;
    }

    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        if (rectangleSize.Width == 0 || rectangleSize.Height == 0)
        {
            throw new ArgumentException("Размер ширины м высоты должен быть больше 0.");
        }

        var rectangle = CreateNewRectangle(rectangleSize);
        rectangle = RectangleCompressions(rectangle);
        rectangles.Add(rectangle);
        return rectangle;
    }

    public IReadOnlyCollection<Rectangle> GetRectangles()
    {
        return rectangles.AsReadOnly();
    }

    private Rectangle CreateNewRectangle(Size rectangleSize)
    {
        var rectangleLocation = GetRectangleLocation(rectangleSize);
        var rectangle = new Rectangle(rectangleLocation, rectangleSize);
        while (CheckRectangleOverlaps(rectangle))
        {
            UpdateAngle();
            rectangleLocation = GetRectangleLocation(rectangleSize);
            rectangle = new Rectangle(rectangleLocation, rectangleSize);
        }

        return rectangle;
    }

    private Rectangle RectangleCompressions(Rectangle rectangle)
    {
        var compressionRectangle = rectangle;
        compressionRectangle = Compression(compressionRectangle,
            (moveRectangle) => moveRectangle.X > center.X,
            (moveRectangle) => moveRectangle.X + rectangle.Width < center.X,
            (moveRectangle, direction) => moveRectangle with {X = moveRectangle.X + direction});

        compressionRectangle = Compression(compressionRectangle,
            (moveRectangle) => moveRectangle.Y > center.Y,
            (moveRectangle) => moveRectangle.Y + moveRectangle.Height < center.Y,
            (moveRectangle, direction) => moveRectangle with {Y = moveRectangle.Y + direction});

        return compressionRectangle;
    }

    private Rectangle Compression(Rectangle rectangle,
        Func<Rectangle, bool> checkPositiveMove,
        Func<Rectangle, bool> checkNegativeMove,
        Func<Rectangle, int, Rectangle> doMove)
    {
        if (checkPositiveMove(rectangle) && checkNegativeMove(rectangle)
            || !checkPositiveMove(rectangle) && !checkNegativeMove(rectangle))
        {
            return rectangle;
        }

        var direction = checkPositiveMove(rectangle) ? -1 : 1;
        var moveRectangle = rectangle;
        while (true)
        {
            moveRectangle = doMove(moveRectangle, direction);
            if (CheckRectangleOverlaps(moveRectangle))
            {
                moveRectangle = doMove(moveRectangle, -1 * direction);
                break;
            }

            if ((direction == -1 && !checkPositiveMove(moveRectangle))
                || (direction == 1 && !checkNegativeMove(moveRectangle)))
            {
                break;
            }
        }

        return moveRectangle;
    }

    private bool CheckRectangleOverlaps(Rectangle rectangle)
    {
        return rectangles.Any(r => r.IntersectsWith(rectangle));
    }

    private Point GetRectangleLocation(Size rectangleSize)
    {
        var shiftFromCenter = -1 * rectangleSize / 2;
        if (!rectangles.Any())
        {
            UpdateLayer();
            return center + shiftFromCenter;
        }

        return CalculateLocationInSpiral() + shiftFromCenter;
    }

    private Point CalculateLocationInSpiral()
    {
        if (!rectangles.Any())
        {
            throw new ArgumentException(
                "Должен быть хотя бы один прямоугольник для вычисления позиции для следующего вне центра спирали.");
        }

        var x = center.X + layer * distanceLayersDifference * Math.Cos(angle);
        var y = center.Y + layer * distanceLayersDifference * Math.Sin(angle);
        var wholeX = Convert.ToInt32(x);
        var wholeY = Convert.ToInt32(y);
        var newLocation = new Point(wholeX, wholeY);
        return newLocation;
    }

    private void UpdateAngle()
    {
        angle += betweenAngleDifference;
        if (angle > fullCircleTurn)
        {
            UpdateLayer();
            angle = 0;
        }
    }

    private void UpdateLayer()
    {
        layer++;
    }
}