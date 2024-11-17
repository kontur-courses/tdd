using System.Drawing;
using TagsCloudVisualization.PointsGenerators;

namespace TagsCloudVisualization.CloudLayouter;

public class CircularCloudLayouter : ICircularCloudLayouter
{
    public Point Center { get; }
    public List<Rectangle> GeneratedRectangles { get; }
    private readonly IPointsGenerator spiral;

    public CircularCloudLayouter(Point center)
    {
        Center = center;
        GeneratedRectangles = new List<Rectangle>();    
        spiral = new SpiralPointsGenerator(center);
    }

    public CircularCloudLayouter(Point center, int step, int angleOffset) : this(center)
    {
        spiral = new SpiralPointsGenerator(center, step, angleOffset);
    }

    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
            throw new ArgumentException($"{nameof(rectangleSize)} height and width must be greater than zero");
        Rectangle rectangle;
        do
        {
            rectangle = GetNextRectangle(rectangleSize);
        } while (GeneratedRectangles.Any(rectangle.IntersectsWith));
        GeneratedRectangles.Add(rectangle);
        return rectangle;
    }

    private Rectangle GetNextRectangle(Size rectangleSize)
    {
        var rectanglePosition = spiral.GetNextPointPosition();
        return CreateRectangle(rectanglePosition, rectangleSize);
    }

    private static Rectangle CreateRectangle(Point center, Size rectangleSize)
    {
        var x = center.X - rectangleSize.Width / 2;
        var y = center.Y - rectangleSize.Height / 2;
        return new Rectangle(x, y, rectangleSize.Width, rectangleSize.Height);
    }
}