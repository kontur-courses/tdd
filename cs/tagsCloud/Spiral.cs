using System.Drawing;

namespace tagsCloud;

public class Spiral
{
    private int counter;
    private readonly float step;
    private Point center;

    public Spiral(Point center, float step = 0.1f)
    {
        this.center = center;
        if (step == 0)
            throw new ArgumentException("angle step non be 0");
        this.step = step;
    }

    public Point GetPoint()
    {
        double x = center.X;
        double y = center.Y;
        var angle = step * counter;
        var xOffset = (float)(step * angle * Math.Cos(angle));
        var yOffset = (float)(step * angle * Math.Sin(angle));
        x += xOffset;
        y += yOffset;
        var point = new Point((int)Math.Round(x), (int)Math.Round(y));
        center = point;
        counter += 1;
        return point;
    }
}