using System.Drawing;

namespace TagsCloudVisualization;

public class ArchimedeanSpiral : ISpiral
{
    private const double DefaultCoefficient = 1d; 
    
    private readonly Point _center;
    private readonly double _coefficient;

    public ArchimedeanSpiral(Point center = default, double coefficient = DefaultCoefficient)
    {
        if (coefficient <= 0)
            throw new ArgumentException("coefficient should be positive", nameof(coefficient));

        _center = center;
        _coefficient = coefficient;
    }

    public Point GetCartesianPoint(int degree)
    {
        if (degree < 0)
            throw new ArgumentException("degree should be positive", nameof(degree));

        var radians = Math.PI * degree / 180;
        var radius = _coefficient * radians;

        var x = (int)(radius * Math.Cos(radians)) + _center.X;
        var y = (int)(radius * Math.Sin(radians)) + _center.Y;
        return new Point(x, y);
    }
}