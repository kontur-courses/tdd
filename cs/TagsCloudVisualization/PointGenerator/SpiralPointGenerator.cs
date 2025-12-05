using System.Drawing;

namespace TagsCloudVisualization.PointGenerator;

public class SpiralPointGenerator : IPointGenerator
{
    private readonly Point center;
    private readonly double radius;
    private readonly double angle;

    public SpiralPointGenerator(Point center, double radius = 2, double angle = double.Pi/40)
    {
        if (center.X < 0 || center.Y < 0)
            throw new ArgumentException("Center coordinates must be non-negative");
        if (radius <= 0)
            throw new ArgumentException("Radius must be positive");
        this.radius = radius;
        this.angle = angle;
        this.center = center;
    }

    public IEnumerable<Point> GetPoints()
    {
        var currentAngle = 0.0;
        while (true)
        {
            var vector = radius * currentAngle / (2 * Math.PI);
            var x = center.X + (int)Math.Round(Math.Cos(currentAngle) * vector);
            var y = center.Y + (int)Math.Round(Math.Sin(currentAngle) * vector);
            yield return new Point(x, y);
            currentAngle += angle;
        }
    }
}