using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization;

public class SpiralGenerator : IPointGenerator
{
    private Point center;
    public Point Center => new(center.X, center.Y);
    public int Radius { get; private set; }
    public int RadiusDelta { get; private set; }
    public double Angle { get; private set; }
    public double AngleDelta { get; private set; }

    public void Initialise(Point initCenter)
    {
        center = initCenter;
        Radius = 0;
        RadiusDelta = 1;
        Angle = 0;
        AngleDelta = Math.PI / 60;
    }

    public Point GetNextPoint()
    {
        var x = (int)Math.Round(center.X + Radius * Math.Cos(Angle));
        var y = (int)Math.Round(center.Y + Radius * Math.Sin(Angle));

        var nextAngle = Angle + AngleDelta;
        var angleMoreThan2Pi = Math.Abs(nextAngle) >= Math.PI * 2;

        Radius = angleMoreThan2Pi ? Radius + RadiusDelta : Radius;
        Angle = angleMoreThan2Pi ? 0 : nextAngle;

        return new Point(x, y);
    }
}