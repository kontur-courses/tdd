using System.Drawing;

namespace TagsCloudVisualization;

public interface IPositionGenerator
{
    public IEnumerable<Point> GetPositions();
}
