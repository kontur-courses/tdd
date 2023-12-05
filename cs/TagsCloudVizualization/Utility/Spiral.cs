using System.Drawing;

namespace TagsCloudVizualization;

public class Spiral
{
    private readonly Point center;
    private readonly double angleStep;
    private readonly double radiusStep;
    private double radius;
    private double angle;

    public Spiral(Point center, double angleStep, double radiusStep)
    {
        if (angleStep <= 0 || radiusStep <= 0)
        {
            throw new ArgumentException("Step values should be positive.");
        }
        this.center = center;
        this.angleStep = angleStep;
        this.radiusStep = radiusStep;
        radius = 0;
        angle = 0;
    }
    public Point GetNextPointOnSpiral()
    {
        var currentPoint = ConvertFromPolarToCartesian();
        angle += angleStep;
        radius += radiusStep;
        return currentPoint;
    }

    private Point ConvertFromPolarToCartesian()
    {
        var x = (int)Math.Round(center.X + Math.Cos(angle) * radius);
        var y = (int)Math.Round(center.Y + Math.Sin(angle) * radius);
        return new Point(x, y);
    }
}