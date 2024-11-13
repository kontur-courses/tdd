using System;
using System.Drawing;

namespace TagsCloudVisualization;

public class ArchimedeanSpiral
{
    private readonly Point center;
    private double angle;
    private readonly double spiralStep;

    public ArchimedeanSpiral(Point center, double spiralStep = 0.1)
    {
        this.center = center;
        this.spiralStep = spiralStep;
        angle = 0;
    }

    public Point GetNextPoint()
    {
        var x = (int)(center.X + angle * Math.Cos(angle));
        var y = (int)(center.Y + angle * Math.Sin(angle));
        angle += spiralStep;
        return new Point(x, y);
    }
}
