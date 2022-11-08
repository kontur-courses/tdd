using System.Drawing;

namespace TagsCloudVisualization;

public class ArithmeticSpiral
{
    private readonly Point _center;
    private readonly double _constant;

    public ArithmeticSpiral(Point center, int constant = 1)
    {
        if (constant <= 0)
            throw new ArgumentException("Negative or zero arithmetic spiral constant not allowed");

        _center = center;
        _constant = constant;
    }

    public Point GetPoint(double length)
    {
        var newX = (int)(_center.X + Math.Cos(length) * length * _constant);
        var newY = (int)(_center.Y + Math.Sin(length) * length * _constant);

        return new Point(newX, newY);
    }
}