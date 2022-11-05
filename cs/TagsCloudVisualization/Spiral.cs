using System.Drawing;

namespace TagsCloudVisualization;

public class Spiral
{
    private readonly Point center;
    private readonly float step;
    private readonly int parameter;
    private float angle;

    public Spiral(Point center, int parameter = 2, float step = 1f)
    {
        if (parameter == 0)
            throw new ArgumentException("Parameter should be not equal zero");
        if (step == 0.0f)
            throw new ArgumentException("Step should be not equal zero");
        this.center = center;
        this.step = step;
        this.parameter = parameter;
    }

    public Point Next()
    {
        if (angle == 0.0f)
        {
            angle += step;
            return center;
        }

        int x = parameter * (int)Math.Round(angle * Math.Cos(angle)) + center.X;
        int y = parameter * (int)Math.Round(angle * Math.Sin(angle)) + center.Y;

        var nextPoint = new Point(x, y);
        angle += step;

        return nextPoint;
    }
}