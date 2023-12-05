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
    private readonly Point center = new(0, 0);
    private int radius;
    private double angle;
    private const int RadiusDelta = 1;
    private const double AngleDelta = Math.PI / 60;

    public Point GetNextPoint()
    {
        var x = (int)Math.Round(center.X + radius * Math.Cos(angle));
        var y = (int)Math.Round(center.Y + radius * Math.Sin(angle));

        var nextAngle = angle + AngleDelta;
        var angleMoreThan2Pi = Math.Abs(nextAngle) >= Math.PI * 2;

        radius = angleMoreThan2Pi ? radius + RadiusDelta : radius;
        angle = angleMoreThan2Pi ? 0 : nextAngle;

        return new Point(x, y);
    }
}