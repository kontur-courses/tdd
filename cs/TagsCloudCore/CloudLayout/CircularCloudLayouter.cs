using System.Drawing;
using TagsCloudCore.CloudLayout.Extensions;

namespace TagsCloudCore.CloudLayout;

public class CircularCloudLayouter(Point center)
{
    private readonly List<Rectangle> rectangles = [];
    public IEnumerable<Rectangle> PlacedRectangles => rectangles;
    public Point Center { get; } = center;

    private double spiralRadianAngle;
    private const double SpiralRadianAngleStep = 0.1;
    private const double SpiralRadiusStep = 0.5;

    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
            throw new ArgumentException("Size must be positive", nameof(rectangleSize));

        Rectangle currentRectangle;

        do
        {
            var upperLeftRectangleCorner = GetNextPossibleRectanglePosition();
            currentRectangle = new Rectangle(upperLeftRectangleCorner, rectangleSize);
        } while (currentRectangle.IntersectsAny(rectangles));

        currentRectangle = ToCenterTightener.Tighten(currentRectangle, Center, rectangles);

        rectangles.Add(currentRectangle);
        return currentRectangle;
    }

    private Point GetNextPossibleRectanglePosition()
    {
        var radius = SpiralRadiusStep * spiralRadianAngle;

        var point = new Point(
            Center.X + (int)(radius * Math.Cos(spiralRadianAngle)),
            Center.Y + (int)(radius * Math.Sin(spiralRadianAngle)));

        spiralRadianAngle += SpiralRadianAngleStep;

        return point;
    }
}