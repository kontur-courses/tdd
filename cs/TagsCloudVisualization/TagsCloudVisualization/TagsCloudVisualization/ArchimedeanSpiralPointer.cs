using System.Drawing;
using TagsCloudVisualization.Interfaces;

namespace TagsCloudVisualization;

public class ArchimedeanSpiralPointer : IFormPointer
{
    private double сurrentDifference;
    private readonly double step;
    private Point center;
    private readonly double dadiusConst;
    private readonly double angleConst;
    private double Angle => сurrentDifference * angleConst;
    private double Radius => сurrentDifference * dadiusConst;

    public ArchimedeanSpiralPointer(Point center, double step, double radiusConst, double angleConst)
    {
        if (step <= 0 || radiusConst <= 0 || angleConst <= 0)
            throw new ArgumentException("either step or radius or angle is not possitive");

        this.center = center;
        сurrentDifference = step;
        this.step = step;
        dadiusConst = radiusConst;
        this.angleConst = angleConst;
    }

    public Point GetNextPoint()
    {
        сurrentDifference += step;
        var x = center.X + (int)(Radius * Math.Cos(Angle));
        var y = center.Y + (int)(Radius * Math.Sin(Angle));

        return new Point(x, y);
    }
}

