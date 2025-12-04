using System.Drawing;

namespace TagCloud;

public class Spiral : ISpiral
{
    private const double DefaultDeltaAngle = 0.05;
    private const double DefaultDeltaRadius = 0.01;

    private readonly Point center;
    private readonly double deltaAngle;
    private readonly double deltaRadius;
    
    public Spiral(Point center, double deltaAngle = DefaultDeltaAngle, double deltaRadius = DefaultDeltaRadius)
    {
        if (deltaRadius <= 0 || deltaAngle <= 0)
        {
            throw new ArgumentException("deltaRadius and deltaAngle must be positive");
        }

        this.center = center;
        this.deltaAngle = deltaAngle;
        this.deltaRadius = deltaRadius;
    }
    
    public Spiral(SpiralSettings spiralSettings) : this(
        spiralSettings.Center, 
        spiralSettings.DeltaAngle, 
        spiralSettings.DeltaRadius)
    {
    }
    
    public Spiral(double deltaAngle = DefaultDeltaAngle, double deltaRadius = DefaultDeltaRadius) : this(
        new Point(0,0), 
        deltaAngle, 
        deltaRadius)
    {
    }

    public IEnumerable<Point> GetPointsOnSpiral()
    {
        var angle = 0.0;
        var radius = 0.0;
        while (true)
        {
            var point = ConvertFromPolarCoordinates(angle, radius);
            point.Offset(center);
            yield return point;
            angle += deltaAngle;
            radius += deltaRadius;
        }
    }

    public static Point ConvertFromPolarCoordinates(double angle, double radius)
    {
        var x = (int)Math.Ceiling(Math.Cos(angle) * radius);
        var y = (int)Math.Ceiling(Math.Sin(angle) * radius);
        return new Point(x, y);
    }
}