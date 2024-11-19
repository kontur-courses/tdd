using System.Drawing;
using TagsCloudVisualization.Extensions;
using TagsCloudVisualization.Interfaces;

namespace TagsCloudVisualization;

public class SpiralPointGenerator : IPointGenerator
{
    private const double AngleStep = Math.PI / 360;
    private readonly double _radiusStep;
    private readonly Size _center;

    public SpiralPointGenerator(Point centerPoint, double radiusStep = 0.01)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(radiusStep);

        _radiusStep = radiusStep;
        _center = new Size(centerPoint);
    }

    public IEnumerable<Point> GeneratePoint()
    {
        var radius = 0d;
        var angle = 0d;

        while (true)
        {
            var newX = (int)(radius * Math.Cos(angle));
            var newY = (int)(radius * Math.Sin(angle));
            var newPoint = new Point(newX, newY).MoveTo(_center);

            radius += _radiusStep;
            angle += AngleStep;

            yield return newPoint;
        }
        // ReSharper disable once IteratorNeverReturns
    }
}