using System.Drawing;

namespace TagsCloudVisualization;

public class SpiralPointGenerator(Point center)
{
    private const double AngleStepRadians = 0.05;
    private const double RadiusGrowthRate = 0.1;
    private double angle;
    private double radius;

    public Point GenerateNextPoint()
    {
        var x = center.X + (int)(radius * Math.Cos(angle));
        var y = center.Y + (int)(radius * Math.Sin(angle));

        angle += AngleStepRadians;
        radius = RadiusGrowthRate * angle;

        return new Point(x, y);
    }
}