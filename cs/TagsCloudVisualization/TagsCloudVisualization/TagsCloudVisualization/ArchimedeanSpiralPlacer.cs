using System.Drawing;
using TagsCloudVisualization.Interfaces;

namespace TagsCloudVisualization;

public class ArchimedeanSpiralPlacer : IRectanglesPlacer
{
    private double CurrentDifference { get; set; }
    private double Step { get; set; }
    public Point Center { get; private set; }
    private double RadiusConst { get; set; }
    private double AngleConst { get; set; }
    public double Angle => CurrentDifference * AngleConst;
    public double Radius => CurrentDifference * RadiusConst;

    public ArchimedeanSpiralPlacer(Point center, double step, double radiusConst, double angleConst)
    {
        if (step <= 0 || radiusConst <= 0 || angleConst <= 0)
            throw new ArgumentException("either step or radius or angle is not possitive");

        Center = center;
        CurrentDifference = step;
        Step = step;
        RadiusConst = radiusConst;
        AngleConst = angleConst;
    }

    public Point GetNextPoint()
    {
        CurrentDifference += Step;
        var x = Center.X + (int)(Radius * Math.Cos(Angle));
        var y = Center.Y + (int)(Radius * Math.Sin(Angle));
        return new Point(x, y);
    }
}

