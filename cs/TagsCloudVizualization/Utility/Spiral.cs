using System.Drawing;
using TagsCloudVizualization.Interfaces;

namespace TagsCloudVizualization.Utility;

public class Spiral : ISpiral
{
    private readonly Point center;
    private readonly double angleStep;
    private readonly double radiusStep;
    private double radius;
    private double angle;

    public Spiral(Point center, double angleStep, double radiusStep)
    {
        if (radiusStep <= 0 || angleStep <= 0)
        {
            throw new ArgumentException("Step values should be positive.");
        }

        this.center = center;
        this.angleStep = angleStep;
        this.radiusStep = radiusStep;
        radius = 0;
        angle = 0;
    }

    public IEnumerable<Point> GetPointsOnSpiral()
    {
        for (double angle = 0, radius = 0; ; angle += angleStep, radius += radiusStep)
        {
            var point = ConvertFromPolarToCartesian(angle, radius);
            point.Offset(center);
            yield return point;
        }
    }

    public static Point ConvertFromPolarToCartesian(double angle, double radius)
    {
        var x = (int)Math.Round(Math.Cos(angle) * radius);
        var y = (int)Math.Round(Math.Sin(angle) * radius);
        return new Point(x, y);
    }
}
