using System.Drawing;

namespace TagsCloudVisualization;

public class CircularCloudLayouter : ICircularCloudLayouter
{
    private readonly Point center;
    private readonly List<Rectangle> rectangles = new();
    private readonly IEnumerator<Point> spiralEnumerator;
    private readonly ICircularCloudLayoutTracer? tracer;

    public CircularCloudLayouter(Point center, ICircularCloudLayoutTracer? tracer = null)
    {
        if (center.IsEmpty || center.X <= 0 || center.Y <= 0)
            throw new ArgumentException("center point is invalid", nameof(center));
        this.center = center;
        this.tracer = tracer;
        spiralEnumerator = CircularHelper.EnumeratePointsInArchimedesSpiral(0.01f, 0.05f, center).GetEnumerator();
    }


    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        var rectangle = Rectangle.Empty;

        rectangle = GetRectangleOnNextSpiralPoint(rectangleSize, rectangle);

        rectangle = TryShiftRectangleCloserToCenter(rectangle);

        tracer?.TraceRectangle(rectangle);

        rectangles.Add(rectangle);

        return rectangle;
    }

    private Rectangle GetRectangleOnNextSpiralPoint(Size rectangleSize, Rectangle rectangle)
    {
        while (spiralEnumerator.MoveNext())
        {
            var currentCirclePoint = spiralEnumerator.Current;
            rectangle = GetCenteredRectangle(rectangleSize, currentCirclePoint);
            tracer?.TraceCirclePoint(currentCirclePoint);
            if (rectangles.All(otherRectangle => !otherRectangle.IntersectsWith(rectangle)))
                break;
        }

        return rectangle;
    }

    private Rectangle TryShiftRectangleCloserToCenter(Rectangle rectangle)
    {
        while (true)
        {
            var shiftRectangle = ShiftDirectlyToCenterByOne(rectangle);
            if (shiftRectangle == rectangle)
                break;

            if (rectangles.Any(x => x.IntersectsWith(shiftRectangle)))
                break;

            tracer?.TraceShifting(rectangle.Center(), shiftRectangle.Center());

            rectangle = shiftRectangle;
        }

        while (true)
        {
            var contiguousRectangles = rectangles
                .Where(x => x.TouchesWith(rectangle))
                .ToArray();

            var shiftRectangle = ShiftCloserToCenterAlongOtherByOne(rectangle, contiguousRectangles);
            if (shiftRectangle == rectangle)
                break;

            if (shiftRectangle == default ||
                rectangles.Any(x => x.IntersectsWith(shiftRectangle)))
                break;

            tracer?.TraceShifting(rectangle.Center(), shiftRectangle.Center());

            rectangle = shiftRectangle;
        }

        return rectangle;
    }

    private Rectangle ShiftCloserToCenterAlongOtherByOne(Rectangle rectangle, Rectangle[] contiguousRectangles)
    {
        var rectangleCenter = rectangle.Center();
        var horizontalShiftedRectangle = rectangle.GetShiftedRectangle(new(
            Math.Clamp(center.X - rectangleCenter.X, -1, 1),
            0));
        if (!contiguousRectangles.Any(x => x.IntersectsWith(horizontalShiftedRectangle)))
            return horizontalShiftedRectangle;

        var verticalShiftedRectangle = rectangle.GetShiftedRectangle(new(
            0,
            Math.Clamp(center.Y - rectangleCenter.Y, -1, 1)));
        if (!contiguousRectangles.Any(x => x.IntersectsWith(verticalShiftedRectangle)))
            return verticalShiftedRectangle;

        return default;
    }

    private static Rectangle GetCenteredRectangle(Size rectangleSize, Point location)
    {
        var centeredLocation =
            new Point(location.X - rectangleSize.Width / 2, location.Y - rectangleSize.Height / 2);
        return new(centeredLocation, rectangleSize);
    }

    private Rectangle ShiftDirectlyToCenterByOne(Rectangle rectangle)
    {
        var rectangleCenter = rectangle.Center();
        var shiftOffset = GetShiftOffset(rectangleCenter);
        return rectangle.GetShiftedRectangle(shiftOffset);
    }

    private Point GetShiftOffset(Point rectangleCenter)
    {
        return new(
            Math.Clamp(center.X - rectangleCenter.X, -1, 1),
            Math.Clamp(center.Y - rectangleCenter.Y, -1, 1));
    }
}