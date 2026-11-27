using System.Drawing;

namespace TagsCloudVisualization;

/// <summary>
/// Хранит размещённые прямоугольники одного облака и позволяет проверять пересечения.
/// </summary>
public interface IPlacedRectangles
{
    /// <summary>
    /// Проверяет, пересекается ли кандидат хотя бы с одним сохранённым прямоугольником.
    /// Касание границ допускается.
    /// </summary>
    bool Intersects(Rectangle candidate);

    /// <summary>
    /// Сохраняет размещённый прямоугольник для последующих проверок.
    /// </summary>
    void Add(Rectangle rectangle);
}
