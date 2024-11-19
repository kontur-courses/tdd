using System.Drawing;


namespace TagsCloudVisualization;

public class CircularCloudLayouter
{
    private const double AngleChangeStep = Math.PI / 180;
    private int DistanceBetweenTurns { get; set; }
    private int InitialRadiusOfSpiral { get; set; }
    private double AngleOfRotationInRadians { get; set; }

    private readonly LinkedList<Rectangle> cloudOfRectangles;

    public readonly Point Center;

    public CircularCloudLayouter(Point center)
    {
        Center = center;
        cloudOfRectangles = [];
        DistanceBetweenTurns = 30;
    }

    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        var halfOfMinSide = Math.Min(rectangleSize.Width, rectangleSize.Height) / 2;
        DistanceBetweenTurns = Math.Min(DistanceBetweenTurns, halfOfMinSide);

        if (cloudOfRectangles.Count == 0) InitialRadiusOfSpiral = halfOfMinSide;

        var rectangle = ChooseLocationForRectangle(rectangleSize);
        cloudOfRectangles.AddFirst(rectangle);

        return rectangle;
    }

    private Rectangle ChooseLocationForRectangle(Size rectangleSize)
    {
        var currentPoint = GetNewPoint();
        var rectangle = GetNewRectangle(currentPoint, rectangleSize);

        while (cloudOfRectangles.Any(rect => rect.IntersectsWith(rectangle)))
        {
            AngleOfRotationInRadians += AngleChangeStep;
            currentPoint = GetNewPoint();
            rectangle = GetNewRectangle(currentPoint, rectangleSize);
        }

        return rectangle;
    }

    private Rectangle GetNewRectangle(Point centerPoint, Size rectangleSize) =>
        new(new(centerPoint.X - rectangleSize.Width / 2,
            centerPoint.Y - rectangleSize.Height / 2), rectangleSize);

    private Point GetNewPoint()
    {
        var coefficient = InitialRadiusOfSpiral + AngleOfRotationInRadians * DistanceBetweenTurns;
        var x = coefficient * Math.Cos(AngleOfRotationInRadians) + Center.X;
        var y = coefficient * Math.Sin(AngleOfRotationInRadians) + Center.Y;

        return new((int)x, (int)y);
    }
}
