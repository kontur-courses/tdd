using System.Drawing;

namespace TagsCloudVisualization.PointsGenerators;

public class SpiralPointsGenerator : IPointsGenerator
{
    private readonly double step;
    private readonly double angleOffset;
    private readonly Point center;
    private double currentAngle = 0;

    public SpiralPointsGenerator(Point center, double step = 0.1, double angleOffset = 0.1)
    {
        if (step == 0 || angleOffset == 0)
            throw new ArgumentException($"Step and angleOffset must not be zero");
        this.center = center;
        this.step = step;
        this.angleOffset = angleOffset;
    }

    public Point GetNextPointPosition()
    {
        var radius = step * currentAngle;
        var x = (int)(center.X + radius * Math.Cos(currentAngle));
        var y = (int)(center.Y + radius * Math.Sin(currentAngle));
        currentAngle += angleOffset;
        return new(x, y);
    }
}