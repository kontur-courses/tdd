using System.Drawing;

namespace TagsCloudVisualization.PointGenerator;

public interface IPointGenerator
{
    public IEnumerable<Point> GetPoints();
}