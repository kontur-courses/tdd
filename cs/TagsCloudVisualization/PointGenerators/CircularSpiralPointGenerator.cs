using System.Drawing;

namespace TagsCloudVisualization.PointGenerators;

public class CircularSpiralPointGenerator : IPointGenerator
{
    private double angleOffset;
    private double radius;
    private double angle = 0;
    private Point center;

    public CircularSpiralPointGenerator(double radius, double angleOffset, Point center)
    {
        if (radius <= 0)
            throw new ArgumentException("radius must be greater than 0");
        if (angleOffset <= 0)
            throw new ArgumentException("angleOffset must be greater than 0");

        this.radius = radius;
        this.angleOffset = angleOffset * Math.PI / 180;
        this.center = center;
    }

    public Point GetPoint()
    {
        var radiusVector = (radius / (2 * Math.PI)) * angle;

        var x = (int)Math.Round(
            radiusVector * Math.Cos(angle) + center.X);
        var y = (int)Math.Round(
            radiusVector * Math.Sin(angle) + center.Y);

        angle += angleOffset;

        return new Point(x, y);
    }
}