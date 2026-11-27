using System.Drawing;

namespace TagsCloudVisualization;

/// <summary>
/// Задаёт порядок перебора позиций относительно центра облака.
/// </summary>
public interface ICandidatePositionSource
{
    /// <summary>
    /// Возвращает новую последовательность смещений от центра облака в порядке проверки.
    /// </summary>
    IEnumerable<Point> GetOffsets();
}
