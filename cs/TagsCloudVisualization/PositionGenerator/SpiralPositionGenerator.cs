using System.Drawing;

namespace TagsCloudVisualization;

public class SpiralPositionGenerator : IPositionGenerator
{
    private const double ANGLE_OFFSET = 0.5;
    private readonly Point center;
    private readonly double step;

    public SpiralPositionGenerator(Point center, double step = 0.1)
    {
        if (step <= 0)
            throw new ArgumentException($"{nameof(step)} должен быть больше нуля, передано {step}");
        this.step = step;
        this.center = center;
    }

    public IEnumerable<Point> GetPositions()
    {
        int x, y;
        double radius, angle = 0;

        while (true)
        {
            radius = step * angle;
            x = (int)(center.X + radius * Math.Cos(angle));
            y = (int)(center.Y + radius * Math.Sin(angle));

            yield return new(x, y);

            angle += ANGLE_OFFSET;
        }
    }
}
