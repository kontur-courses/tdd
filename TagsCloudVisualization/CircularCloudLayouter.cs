using System.Drawing;

namespace TagsCloudVisualization;

public class CircularCloudLayouter
{
    private const double SpiralAngleStep = 0.1;
    private const double SpiralRadiusGrowth = 1.0;

    private readonly Point center;
    private readonly List<Rectangle> placedRectangles = [];

    /// <summary>
    /// Создаёт раскладчик с заданным центром облака.
    /// </summary>
    public CircularCloudLayouter(Point center)
    {
        this.center = center;
    }

    /// <summary>
    /// Определяет позицию следующего прямоугольника в облаке и возвращает его с заданным размером.
    /// </summary>
    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        for (var angle = 0.0; ; angle += SpiralAngleStep)
        {
            var radius = SpiralRadiusGrowth * angle;
            var offsetX = (int)Math.Round(radius * Math.Cos(angle));
            var offsetY = (int)Math.Round(radius * Math.Sin(angle));
            var location = new Point(
                center.X + offsetX - rectangleSize.Width / 2,
                center.Y + offsetY - rectangleSize.Height / 2);
            var candidate = new Rectangle(location, rectangleSize);

            if (placedRectangles.Any(rectangle => rectangle.IntersectsWith(candidate)))
                continue;

            placedRectangles.Add(candidate);
            return candidate;
        }
    }
}
