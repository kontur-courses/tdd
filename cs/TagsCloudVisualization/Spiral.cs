using System.Drawing;

namespace TagsCloudVisualization;

public class Spiral(Point start, double step)
{
    private double angle;

    public Point GetNextPoint()
    {
        var x = (int)(start.X + step * angle * Math.Cos(angle));
        var y = (int)(start.Y + step * angle * Math.Sin(angle));
        angle += Math.PI / 40;

        return new Point(x, y);
    }
}