using System.Drawing;

namespace TagsCloudVisualization;

/// <summary>
/// Последовательно размещает прямоугольники в облаке.
/// </summary>
public interface ICloudLayouter
{
    /// <summary>
    /// Размещает следующий прямоугольник заданного размера и возвращает его координаты и размер.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">Ширина или высота меньше либо равна нулю.</exception>
    Rectangle PutNextRectangle(Size rectangleSize);
}
