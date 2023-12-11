using System.Drawing;
using TagsCloudVisualization.Interfaces;

namespace TagsCloudVisualization;

public class ArchimedeanSpiralPointer : IFormPointer
{
    private readonly double _angleConst;
    private readonly Point _center;
    private readonly double _radiusConst;
    private readonly double _step;
    private double _сurrentDifference;

    public ArchimedeanSpiralPointer(Point center, double step, double radiusConst, double angleConst)
    {
        if (step <= 0 || radiusConst <= 0 || angleConst <= 0)
            throw new ArgumentException("either step or radius or angle is not possitive");

        _center = center;
        _сurrentDifference = step;
        _step = step;
        _radiusConst = radiusConst;
        _angleConst = angleConst;
    }

    private double Angle => _сurrentDifference * _angleConst;
    private double Radius => _сurrentDifference * _radiusConst;

    public Point GetNextPoint()
    {
        _сurrentDifference += _step;
        var x = _center.X + (int)(Radius * Math.Cos(Angle));
        var y = _center.Y + (int)(Radius * Math.Sin(Angle));

        return new Point(x, y);
    }
}