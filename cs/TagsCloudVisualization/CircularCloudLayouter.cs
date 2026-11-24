using System.Drawing;

namespace TagsCloudVisualization;

/// <summary>
/// Формирует облако непересекающихся прямоугольников
/// с помощью спирали Архимеда.
/// </summary>
public class CircularCloudLayouter
{
    private readonly Point _center;
    private readonly double _radiusFactor;
    private readonly double _angleStep;

    private readonly List<Rectangle> _rectangles = new();
    public IReadOnlyList<Rectangle> Rectangles => _rectangles.AsReadOnly();

    public CircularCloudLayouter(
        Point center,
        double radiusFactor = 0.5,
        double angleStep = 0.05)
    {
        if (!double.IsFinite(radiusFactor) || radiusFactor <= 0)
            throw new ArgumentOutOfRangeException(nameof(radiusFactor));

        if (!double.IsFinite(angleStep) || angleStep <= 0)
            throw new ArgumentOutOfRangeException(nameof(angleStep));

        _center = center;
        _radiusFactor = radiusFactor;
        _angleStep = angleStep;
    }

    /// <summary>
    /// Размещает прямоугольник в первой свободной позиции вдоль спирали.
    /// </summary>
    /// <param name="rectangleSize">Размер размещаемого прямоугольника.</param>
    /// <returns>Размещённый прямоугольник с рассчитанными координатами.</returns>
    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
            throw new ArgumentOutOfRangeException(nameof(rectangleSize));

        foreach (var point in GetSpiralPoints())
        {
            var rectangle = new Rectangle(
                point.X - rectangleSize.Width / 2,
                point.Y - rectangleSize.Height / 2,
                rectangleSize.Width,
                rectangleSize.Height);

            if (_rectangles.Any(r => r.IntersectsWith(rectangle)))
                continue;

            _rectangles.Add(rectangle);

            return rectangle;
        }

        throw new InvalidOperationException();
    }

    /// <summary>
    /// Лениво генерирует точки спирали Архимеда, начиная с центра облака.
    /// </summary>
    /// <returns>Бесконечная последовательность точек с целочисленными координатами.</returns>
    private IEnumerable<Point> GetSpiralPoints()
    {
        for (var angle = 0.0; ; angle += _angleStep)
        {
            var radius = _radiusFactor * angle;

            var x = _center.X + radius * Math.Cos(angle);
            var y = _center.Y + radius * Math.Sin(angle);

            yield return new Point(
                (int)Math.Round(x),
                (int)Math.Round(y));
        }
    }
}
