using System.Drawing;
using TagsCloudVisualization.Interfaces;

namespace TagsCloudVisualization;

public class ArchimedeanSpiral : ISpiral
{
    private const double DefaultCoefficient = 1d;

    private readonly Point center;
    private readonly double coefficient;

    public ArchimedeanSpiral(Point center = default, double coefficient = DefaultCoefficient)
    {
        if (coefficient <= 0)
            throw new ArgumentException("coefficient should be positive", nameof(coefficient));

        this.center = center;
        this.coefficient = coefficient;
    }

    public Point GetCartesianPoint(int degree)
    {
        if (degree < 0)
            throw new ArgumentException("degree should be positive", nameof(degree));

        var radians = Math.PI * degree / 180;
        var radius = coefficient * radians;

        var x = (int)(radius * Math.Cos(radians)) + center.X;
        var y = (int)(radius * Math.Sin(radians)) + center.Y;
        return new Point(x, y);
    }
}