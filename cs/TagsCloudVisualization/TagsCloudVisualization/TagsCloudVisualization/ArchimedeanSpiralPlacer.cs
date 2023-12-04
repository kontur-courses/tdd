using System.Drawing;
using TagsCloudVisualization.Interfaces;

namespace TagsCloudVisualization;

public class ArchimedeanSpiralPlacer : IRectanglesPlacer
{
    public double CurrentDifference { get; private set; }
    public double Step { get; private set; }
    public Point Center { get; private set; }
    public double RadiusConst { get; private set; }
    public double AngleConst { get; private set; }
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

    public Rectangle GetNextRectangle(Size rectangleSize)
    {
        if (rectangleSize.Height <= 0 || rectangleSize.Width <= 0)
            throw new ArgumentException("either width or height of rectangle size is not possitive");

        CurrentDifference += Step;
        var x = Center.X + (int)(Radius * Math.Cos(Angle));
        var y = Center.Y + (int)(Radius * Math.Sin(Angle));
        return new Rectangle(new Point(x, y), rectangleSize);
    }
}

