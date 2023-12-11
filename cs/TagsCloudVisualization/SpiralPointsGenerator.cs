using System.Drawing;

namespace TagsCloudVisualization;

public sealed class SpiralPointsGenerator : IPointsGenerator
{
    private readonly Point center;
    private readonly double radiusStep;

    public SpiralPointsGenerator(Point center, double radiusStep = 1)
    {
        this.center = center;
        this.radiusStep = radiusStep;
    }

    ///<exception cref="System.OverflowExeption">
    /// Long point generation operation
    /// </exception>
    public IEnumerable<Point> GetPoints()
    {
        double radius = 0;
        var term = radiusStep / 360;
        while (true)
        {
            for (var pointNumber = 0; pointNumber < 360; pointNumber++)
            {
                var pointAngle = 2 * Math.PI * pointNumber / 360;
                // yield return new Point((int)(Math.Cos(pointAngle) * radius) + center.X,
                //     (int)(Math.Sin(pointAngle) * radius) + center.Y);
                var currentPoint = new Point(center.X, center.Y);
                currentPoint.Offset((int)(Math.Cos(pointAngle) * radius), (int)(Math.Sin(pointAngle) * radius));
                yield return currentPoint;
                radius += term;
            }
        }
    }
}