using System.Drawing;

namespace TagsCloudVisualization.Layout;

public interface IPlacementPointSource
{
    IEnumerable<Point> GetPoints();
}