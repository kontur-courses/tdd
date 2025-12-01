using System.Drawing;
using TagsCloudLib.Abstractions;

namespace TagsCloudLib.Implementations;

public class ArchimedeanSpiral(Point center, double spiralStep = 0.2, double radiusStep = 0.5)
    : ISpiral
{
    private double angle;

    public Point GetNextPoint()
    {
        angle += spiralStep;
        var radius = radiusStep * angle;
        var x = (int)(center.X + radius * Math.Cos(angle));
        var y = (int)(center.Y + radius * Math.Sin(angle));
        return new Point(x, y);
    }
}