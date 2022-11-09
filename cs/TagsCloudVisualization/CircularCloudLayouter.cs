using System.Drawing;

namespace TagsCloudVisualization;

public class CircularCloudLayouter : ICircularCloudLayouter
{
    private const int MaximumShiftTriesCount = 100;

    private readonly Point center;
    private readonly int maximumCircularExitViewBoardCount;

    private readonly List<Rectangle> rectangles = new();

    private readonly Rectangle viewBoard;

    private int circularExitViewBoardCount;

    private IEnumerator<Point> spiralEnumerator;

    private bool spiralIsTurned;


    public CircularCloudLayouter(Point center)
    {
        if (center.IsEmpty || center.X <= 0 || center.Y <= 0)
            throw new ArgumentException("center point is invalid", nameof(center));
        this.center = center;
        viewBoard = new(Point.Empty, new Size(center) * 2);
        maximumCircularExitViewBoardCount =
            (int)(Math.Sqrt(viewBoard.Width * viewBoard.Width + viewBoard.Height * viewBoard.Height) * 2);
        spiralEnumerator = CircularHelper.EnumeratePointsInArchimedesSpiral(0.5f, 0.5f, center).GetEnumerator();
    }


    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        if (rectangleSize.Width > viewBoard.Width || rectangleSize.Height > viewBoard.Height)
            throw new ArgumentException("rectangleSize is greater than view board", nameof(rectangleSize));

        var rectangle = Rectangle.Empty;

        rectangle = GetRectangleOnNextSpiralPoint(rectangleSize, rectangle);

        rectangle = TryShiftRectangleCloserToCenter(rectangle);

        rectangles.Add(rectangle);

        return rectangle;
    }

    private Rectangle GetRectangleOnNextSpiralPoint(Size rectangleSize, Rectangle rectangle)
    {
        while (spiralEnumerator.MoveNext())
        {
            rectangle = GetCenteredRectangle(rectangleSize, spiralEnumerator.Current);
            if (!viewBoard.Contains(rectangle))
                CheckOverflowOrTryTurnSpiral();
            else if (rectangles.All(otherRectangle => !otherRectangle.IntersectsWith(rectangle)))
                break;
        }

        return rectangle;
    }

    private void CheckOverflowOrTryTurnSpiral()
    {
        if (++circularExitViewBoardCount <= maximumCircularExitViewBoardCount)
            return;
        if (spiralIsTurned)
            throw new InvalidOperationException("view board is overflowed");
        spiralEnumerator = CircularHelper.EnumeratePointsInArchimedesSpiral(0.5f, 0.5f, center, MathF.PI)
            .GetEnumerator();
        spiralIsTurned = true;
        circularExitViewBoardCount = 0;
    }

    private Rectangle TryShiftRectangleCloserToCenter(Rectangle rectangle)
    {
        for (var i = 0; i < MaximumShiftTriesCount; i++)
        {
            var shiftRectangle = ShiftRectangleCloserToCenterByOne(rectangle);
            var checkRectangleWithAny = rectangles.Any(x => x.IntersectsWith(shiftRectangle));
            if (checkRectangleWithAny)
                break;
            if (shiftRectangle == rectangle)
                break;
            rectangle = shiftRectangle;
        }

        return rectangle;
    }

    private static Rectangle GetCenteredRectangle(Size rectangleSize, Point location)
    {
        var centeredLocation =
            new Point(location.X - rectangleSize.Width / 2, location.Y - rectangleSize.Height / 2);
        return new(centeredLocation, rectangleSize);
    }

    private Rectangle ShiftRectangleCloserToCenterByOne(Rectangle rectangle)
    {
        var rectangleCenterX = rectangle.X + rectangle.Width / 2;
        var rectangleCenterY = rectangle.Y + rectangle.Height / 2;
        var shiftOffset = GetShiftOffset(rectangleCenterX, rectangleCenterY);
        var shiftPosition = rectangle.Location;
        shiftPosition.Offset(shiftOffset);
        var shiftRectangle = new Rectangle(shiftPosition, rectangle.Size);
        return shiftRectangle;
    }

    private Point GetShiftOffset(int rectangleCenterX, int rectangleCenterY)
    {
        var shiftOffset = Point.Empty;
        if (rectangleCenterX < center.X)
            shiftOffset.Offset(1, 0);
        else if (rectangleCenterX > center.X)
            shiftOffset.Offset(-1, 0);
        if (rectangleCenterY < center.Y)
            shiftOffset.Offset(0, 1);
        else if (rectangleCenterY > center.Y)
            shiftOffset.Offset(0, -1);
        return shiftOffset;
    }
}