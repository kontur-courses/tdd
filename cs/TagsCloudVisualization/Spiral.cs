using System.Drawing;

namespace TagsCloudVisualization;

public class Spiral
{
    private readonly Point center;
    private double currentAngle;
    private readonly double angleStep;
    private readonly double radiusMultiplier;

    public Spiral(Point center, double angleStep = 0.1, double radiusMultiplier = 0.5)
    {
        this.center = center;
        this.angleStep = angleStep;
        this.radiusMultiplier = radiusMultiplier;
        currentAngle = 0;
    }

    public Point GetNextPoint()
    {
        var radius = radiusMultiplier * currentAngle;
        var x = center.X + (int)(radius * Math.Cos(currentAngle));
        var y = center.Y + (int)(radius * Math.Sin(currentAngle));

        currentAngle += angleStep;
        return new Point(x, y);
    }
}
