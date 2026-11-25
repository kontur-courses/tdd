using System.Drawing;

public sealed class ArchimedeanSpiral
{
    private readonly Point center;
    private readonly double radiusStep;   
    private readonly double angleStep;

    private double angle;
    private Point current;

    public ArchimedeanSpiral(Point center, double radiusStep = 0.5, double angleStep = Math.PI / 32)
    {
        if (radiusStep <= 0) throw new ArgumentOutOfRangeException(nameof(radiusStep));
        if (angleStep <= 0) throw new ArgumentOutOfRangeException(nameof(angleStep));

        center = center;
        radiusStep = radiusStep;
        angleStep = angleStep;
        Reset();
    }

    public void Reset()
    {
        angle = 0;
        current = center;
    }

    public Point Next()
    {
        angle += angleStep;
        var r = radiusStep * angle / (2 * Math.PI);  
        current = new Point((int)(center.X + r * Math.Cos(angle)),
                             (int)(center.Y + r * Math.Sin(angle)));
        return current;
    }
}