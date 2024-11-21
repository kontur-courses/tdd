using System.Drawing;

namespace TagsCloudVisualization.CloudClasses.Interfaces
{
    // Интерфейс для класса, который перемещает луч по спирали, генерируя последовательность точек.
    public interface IRayMover
    {
        IEnumerable<Point> MoveRay();
    }
}
