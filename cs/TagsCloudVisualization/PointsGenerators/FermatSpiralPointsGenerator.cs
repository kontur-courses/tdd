using System.Drawing;

namespace TagsCloudVisualization.PointsGenerators;

public class FermatSpiralPointsGenerator : IPointsGenerator
{
    private readonly double angleOffset;
    private readonly double radius;
    
    private double OffsetPerRadian => radius / (2 * Math.PI);

    public FermatSpiralPointsGenerator(double radius, double angleOffset)
    {
        if (radius <= 0)
            throw new ArgumentException("radius must be greater than 0", nameof(radius));
        if (angleOffset <= 0)
            throw new ArgumentException("angleOffset must be greater than 0", nameof(angleOffset));
        
        this.angleOffset = angleOffset * Math.PI / 180;
        this.radius = radius;
    }

    public IEnumerable<Point> GeneratePoints(Point spiralCenter)
    {
        double angle = 0;

        while (true)
        {
            yield return GetPointByPolarCoordinates(spiralCenter, angle);
            angle += angleOffset;
        }
    }

    private Point GetPointByPolarCoordinates(Point spiralCenter, double angle)
    {
        var radiusVector = OffsetPerRadian * angle;
        
        var x = (int)Math.Round(
            radiusVector * Math.Cos(angle) + spiralCenter.X);
        var y = (int)Math.Round(
            radiusVector * Math.Sin(angle) + spiralCenter.Y);
        
        return new Point(x, y);
    }
}