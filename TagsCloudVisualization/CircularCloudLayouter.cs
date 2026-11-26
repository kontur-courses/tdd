using System.Drawing;

namespace TagsCloudVisualization;

public class CircularCloudLayouter
{
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
        var location = new Point(
            center.X - rectangleSize.Width / 2,
            center.Y - rectangleSize.Height / 2);
        var candidate = new Rectangle(location, rectangleSize);

        while (placedRectangles.Any(rectangle => rectangle.IntersectsWith(candidate)))
        {
            candidate.Offset(1, 0);
        }

        placedRectangles.Add(candidate);
        return candidate;
    }
}
