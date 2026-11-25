using System.Drawing;
using TagsCloudVisualization.Abstractions;

public sealed class ArchimedeanSpiral : ISpiral
{
    private readonly Point center;
    private readonly double radiusStep;   
    private readonly double angleStep;

    private double angle;
    public Point Current { get; set; }

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
        Current = center;
    }

    public Point Next()
    {
        angle += angleStep;
        var r = radiusStep * angle / (2 * Math.PI);  
        Current = new Point((int)(center.X + r * Math.Cos(angle)),
                             (int)(center.Y + r * Math.Sin(angle)));
        return Current;
    }
}