using System.Drawing;
using TagsCloudVisualization.Abstractions;

namespace TagsCloudVisualization;

public class SpiralPointGenerator : IPointGenerator
{
    private readonly Point center;
    private readonly double step;
    private readonly int parameter;
    private double angle;

    public SpiralPointGenerator(Point center, int parameter = 1, double step = 0.02)
    {
        if (parameter == 0)
            throw new ArgumentException("Parameter should be not equal zero");
        if (step == 0.0)
            throw new ArgumentException("Step should be not equal zero");
        this.center = center;
        this.step = step;
        this.parameter = parameter;
    }

    public Point Next()
    {
        if (angle == 0.0)
        {
            angle += step;
            return center;
        }

        int x = Convert.ToInt32(parameter * angle * Math.Cos(angle) + center.X);
        int y = Convert.ToInt32(parameter * angle * Math.Sin(angle) + center.Y);
        angle += step;

        return new Point(x, y);
    }
}