using System.Drawing;

namespace TagsCloudVisualization;

public class CircularCloudLayouter : ICloudLayouter
{
    private readonly Point center;
    private readonly ICandidatePositionSource positionSource;
    private readonly IPlacedRectangles placedRectangles;

    /// <summary>
    /// Создаёт раскладчик с заданным центром облака.
    /// </summary>
    public CircularCloudLayouter(Point center) : this(center, new ArchimedeanSpiral(), new PlacedRectangleList())
    {
    }

    /// <summary>
    /// Создаёт раскладчик с заданным источником позиций и хранилищем прямоугольников для этого облака.
    /// </summary>
    public CircularCloudLayouter(Point center, ICandidatePositionSource positionSource, IPlacedRectangles placedRectangles)
    {
        this.center = center;
        this.positionSource = positionSource ?? throw new ArgumentNullException(
            nameof(positionSource), "Источник позиций должен быть задан.");
        this.placedRectangles = placedRectangles ?? throw new ArgumentNullException(
            nameof(placedRectangles), "Хранилище размещённых прямоугольников должно быть задано.");
    }

    /// <summary>
    /// Определяет позицию следующего прямоугольника в облаке и возвращает его с заданным размером.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">Ширина или высота прямоугольника меньше либо равна нулю.</exception>
    /// <exception cref="InvalidOperationException">Источник позиций исчерпан, а свободное место не найдено.</exception>
    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(rectangleSize), rectangleSize, "Высота и ширина прямоугольника должны быть больше 0.");
        }

        foreach (var offset in positionSource.GetOffsets())
        {
            var location = new Point(
                center.X + offset.X - rectangleSize.Width / 2,
                center.Y + offset.Y - rectangleSize.Height / 2);
            var candidate = new Rectangle(location, rectangleSize);

            if (placedRectangles.Intersects(candidate))
                continue;

            placedRectangles.Add(candidate);
            return candidate;
        }

        throw new InvalidOperationException("Источник позиций исчерпан, свободное место для прямоугольника не найдено.");
    }
}
