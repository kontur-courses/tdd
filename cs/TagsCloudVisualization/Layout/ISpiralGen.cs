using System.Drawing;

namespace TagsCloudVisualization.Layout;

public interface ISpiralGen
{
    Point Center { get; }

    IEnumerable<Point> GetPoints();
}