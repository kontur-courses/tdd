using System.Drawing;

namespace TagsCloudVisualization;

internal class ArchimedeanSpiralPointGenerator(Point start, float density) : IPointGenerator
{
    private Point _start = start;
    private float _step = 0f;
    private const uint MaxNumberOfPoints = UInt32.MaxValue;
    private uint numberOfReturnedPoints = 0;

    public IEnumerable<Point> GetNextPoint()
    {
        yield return _start;
        while (numberOfReturnedPoints < MaxNumberOfPoints)
        {
            var newX = _start.X + density * _step * (float)Math.Cos(_step);
            var newY = _start.Y + density * _step * (float)Math.Sin(_step);
            yield return Point.Round(new PointF(newX, newY));
            _step += 0.01f;
            ++numberOfReturnedPoints;
        }
    }
}