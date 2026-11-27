using System.Drawing;

namespace TagsCloudVisualization;

/// <summary>
/// Хранит прямоугольники в списке и проверяет пересечения перебором.
/// </summary>
public sealed class PlacedRectangleList : IPlacedRectangles
{
    private readonly List<Rectangle> rectangles = [];

    /// <inheritdoc />
    public bool Intersects(Rectangle candidate)
    {
        return rectangles.Any(rectangle => rectangle.IntersectsWith(candidate));
    }

    /// <inheritdoc />
    public void Add(Rectangle rectangle)
    {
        rectangles.Add(rectangle);
    }
}
