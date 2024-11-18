using System.Drawing;
using TagsCloudVisualization.PointGenerators;

namespace TagsCloudVisualization.Layouters;

public class CircularCloudLayouter : ICloudLayouter
{
    private const double DefaultRadius = 1;
    private const double DefaultAngleOffset = 0.5;
    private readonly Point center;
    public readonly List<Rectangle> rectangles;
    private readonly CircularSpiralPointGenerator pointsGenerator;

    public CircularCloudLayouter(Point center) : this(center, DefaultRadius, DefaultAngleOffset) {}

    public CircularCloudLayouter(Point center, double radius, double angleOffset)
    {
        if (center.X < 0 || center.Y < 0)
            throw new ArgumentException("X or Y must be positive");

        this.center = center;
        rectangles = new List<Rectangle>();

        pointsGenerator = new CircularSpiralPointGenerator(radius, angleOffset, center);
    }

    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        if (rectangleSize.Height <= 0 || rectangleSize.Width <= 0)
            throw new ArgumentException("Height or Width is negative!");

        Rectangle rectangle;

        do
        {
            var rectangleCenterPos = pointsGenerator.GetPoint();
            rectangle = CreateRectangle(rectangleCenterPos, rectangleSize);
        } 
        while (rectangles.Any(rectangle.IntersectsWith));

        rectangles.Add(rectangle);

        return rectangle;
    }

    private static Rectangle CreateRectangle(Point center, Size rectangleSize)
    {
        var x = center.X - rectangleSize.Width / 2;
        var y = center.Y - rectangleSize.Height / 2;
        return new Rectangle(x, y, rectangleSize.Width, rectangleSize.Height);
    }
}