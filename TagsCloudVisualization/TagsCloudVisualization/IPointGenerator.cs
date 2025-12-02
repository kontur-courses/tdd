using System.Drawing;

namespace TagsCloudVisualization;

internal interface IPointGenerator
{
    public IEnumerable<Point> GetNextPoint();
}