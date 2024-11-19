using System.Drawing;

namespace TagsCloudVisualization.Interfaces;

public interface IPointGenerator
{
    public IEnumerable<Point> GeneratePoint();
}