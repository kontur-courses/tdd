using System.Drawing;

namespace TagsCloudVisualization;

public class CircularCloudLayouter : ICloudLayouter
{
    private readonly List<Rectangle> placedRectangles = new();
    private readonly Point center;
    private static readonly Random Random = new();
    
    public CircularCloudLayouter(Point center)
    {
        this.center = center;
    }

    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        if (rectangleSize.Height < 0 || rectangleSize.Width < 0)
            throw new ArgumentException("Height and width must be positive");
            
        Rectangle newRectangle;
        do
        {
            var distanceFromCenter = NextNormalDistribution(10, 50);
            var angle = NextNormalDistribution(0, 360);

            var x = center.X + (int)(distanceFromCenter * Math.Cos(angle));
            var y = center.Y + (int)(distanceFromCenter * Math.Sin(angle));
            newRectangle = new Rectangle(new Point(x, y), rectangleSize);

        } while (IntersectsWithAny(newRectangle));

        placedRectangles.Add(newRectangle);
        return newRectangle;
    }

    private bool IntersectsWithAny(Rectangle newRectangle)
    {
        return placedRectangles
            .Any(rectangle => rectangle
                .IntersectsWith(newRectangle));
    }

    private static int NextNormalDistribution(int mean, int stdDev)
    {
        var u1 = 1.0 - Random.NextDouble();
        var u2 = 1.0 - Random.NextDouble();
        var randomNumber = Math.Sqrt(-2 * Math.Log(u1)) * Math.Sin(2 * Math.PI * u2);
        return mean + (int)(stdDev * randomNumber);
    }
}