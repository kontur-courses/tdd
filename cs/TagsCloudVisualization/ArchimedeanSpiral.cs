using System.Drawing;

namespace TagsCloudVisualization;

public class ArchimedeanSpiral
{
    private readonly float angleStep;
    private readonly Point center;
    private readonly float spiralStep;
    private double angle;

    public ArchimedeanSpiral(Point center, float angleStep = 0.1f, float spiralStep = 0.5f)
    {
        if (angleStep <= 0)
            throw new ArgumentException("Angle step must be positive", nameof(angleStep));

        if (spiralStep <= 0)
            throw new ArgumentException("Radius step must be positive", nameof(spiralStep));

        this.center = center;
        this.angleStep = angleStep;
        this.spiralStep = spiralStep;
    }

    public Point GetNextPoint()
    {
        var radius = spiralStep * angle;
        var x = center.X + (int)(radius * Math.Cos(angle));
        var y = center.Y + (int)(radius * Math.Sin(angle));
        angle += angleStep;

        return new Point(x, y);
    }

    public void Reset()
    {
        angle = 0;
    }
}