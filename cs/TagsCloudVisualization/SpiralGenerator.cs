using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization;

public class SpiralGenerator
{
    private readonly Point center;
    public readonly int RadiusDelta;
    public readonly double AngleDelta;

    public Point Center => new(center.X, center.Y);
    public int Radius { get; private set; }
    public double Angle { get; private set; }

    public SpiralGenerator(int radiusDelta = 1, double angleDelta = Math.PI / 60)
    {
        this.center = new Point(0, 0);
        this.RadiusDelta = radiusDelta < 0 ? throw new ArgumentException("radiusDelta cant be negative") : radiusDelta;
        this.AngleDelta = angleDelta;
    }
    public SpiralGenerator(Point center, int radiusDelta = 1, double angleDelta = Math.PI / 60)
    {
        this.center = center;
        this.RadiusDelta = radiusDelta < 0 ? throw new ArgumentException("radiusDelta cant be negative") : radiusDelta;
        this.AngleDelta = angleDelta;
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

    public void ResetSpiral()
    {
        Radius = 0;
        Angle = 0;
    }
}