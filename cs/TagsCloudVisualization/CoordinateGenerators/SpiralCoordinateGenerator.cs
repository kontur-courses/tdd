using System.Drawing;

namespace TagsCloudVisualization.CoordinateGenerators;

public class SpiralCoordinateGenerator : ICoordinateGenerator
{
    private readonly Point _center;
    private double _angle;
    private readonly double _step;

    public SpiralCoordinateGenerator(Point center, double step)
    {
        _center = center;
        _angle = 0;
        _step = step;
    }

    public Point GetNextPosition()
    {
        var radius = _step * _angle;
        var x = _center.X + radius * Math.Cos(_angle);
        var y = _center.Y + radius * Math.Sin(_angle);

        _angle += _step;

        return new Point((int)x, (int)y);
    }
}