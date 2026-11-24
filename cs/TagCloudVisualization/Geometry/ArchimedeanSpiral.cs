// Geometry/ArchimedeanSpiral.cs

using System.Drawing;

namespace TagCloudVisualization.Geometry;

public class ArchimedeanSpiral
{
    private readonly Point center;
    private readonly double radiusStep;   
    private readonly double angleStep;    

    public ArchimedeanSpiral(Point center, double radiusStep = 1, double angleStep = 0.1)
    {
        if (radiusStep <= 0 || angleStep <= 0)
            throw new ArgumentException("Steps must be positive");
        (this.center, this.radiusStep, this.angleStep) = (center, radiusStep, angleStep);
    }

    public IEnumerable<Point> GetPoints()
    {
        for (var angle = 0.0; ; angle += angleStep)
        {
            var r = radiusStep * angle / (2 * Math.PI);
            yield return new Point(
                center.X + (int)Math.Round(r * Math.Cos(angle)),
                center.Y + (int)Math.Round(r * Math.Sin(angle)));
        }
    }
}