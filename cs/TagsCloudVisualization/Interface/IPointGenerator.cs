using System.Drawing;

namespace TagsCloudVisualization.Interface;

public interface IPointGenerator
{
    IEnumerable<Point> GeneratePoints();
    Point Center { get; }
}