using System.Drawing;

namespace TagsCloudVisualization;

public interface IPointGenerator
{
    IEnumerable<Point> Generate(Point center);
}