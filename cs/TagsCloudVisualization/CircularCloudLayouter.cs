using System.Drawing;

namespace TagsCloudVisualization;

public class CircularCloudLayouter : ICloudLayouter
{
    public List<Rectangle> PlacedRectangles { get; } = new();
    private readonly Point center;
    private static readonly Random Random = new();

    public CircularCloudLayouter(Point center)
    {
        this.center = center;
    }

    private Point CalculateRectangleLocation(Size rectangleSize)
    {
        var distanceFromCenter = NextNormalDistribution(10, Math.Max(rectangleSize.Height, rectangleSize.Width) * 2);
        var angle = NextNormalDistribution(0, 360);

        var x = center.X + (int) (distanceFromCenter * Math.Cos(angle));
        var y = center.Y + (int) (distanceFromCenter * Math.Sin(angle));

        return new Point(x, y);
    }
    
    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        if (rectangleSize.Height < 0 || rectangleSize.Width < 0)
            throw new ArgumentException("Height and width must be positive");

        Rectangle newRectangle;
        do
        {
            var location = CalculateRectangleLocation(rectangleSize);
            newRectangle = new Rectangle(location, rectangleSize);
        } while (IntersectsWithAny(newRectangle));

        PlacedRectangles.Add(newRectangle);
        return newRectangle;
    }

    private bool IntersectsWithAny(Rectangle newRectangle)
    {
        return PlacedRectangles
            .Any(rectangle => rectangle
                .IntersectsWith(newRectangle));
    }

    private static double NextNormalDistribution(double mean, double stdDev)
    {
        var u1 = 1.0 - Random.NextDouble();
        var u2 = 1.0 - Random.NextDouble();
        var randomNumber = Math.Sqrt(-2 * Math.Log(u1)) * Math.Sin(2 * Math.PI * u2);
        return mean + (int) (stdDev * randomNumber);
    }
}