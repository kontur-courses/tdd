using System.Drawing;

namespace TagsCloudVisualization.Layout;

public class ArchimedeanSpiralGen : ISpiralGen
{
    public ArchimedeanSpiralGen(Point center, double spiralStep = 0.5, double angleStep = 0.05)
    {
        Center = center;
        SpiralStep = spiralStep;
        AngleStep = angleStep;
    }

    public double SpiralStep { get; }

    public double AngleStep { get; }
    public Point Center { get; }

    public IEnumerable<Point> GetPoints()
    {
        Point? previousPoint = null;
        var angle = 0.0;

        while (true)
        {
            var radius = SpiralStep * angle;
            var x = Center.X + (int)Math.Round(radius * Math.Cos(angle));
            var y = Center.Y + (int)Math.Round(radius * Math.Sin(angle));
            var currentPoint = new Point(x, y);

            if (currentPoint != previousPoint)
            {
                yield return currentPoint;
                previousPoint = currentPoint;
            }

            angle += AngleStep;
        }
    }
}