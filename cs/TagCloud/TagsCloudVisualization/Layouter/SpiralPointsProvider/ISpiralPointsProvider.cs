using System.Drawing;

namespace TagsCloudVisualization;

public interface ISpiralPointsProvider
{
    IEnumerable<Point> GetSpiralPoints(int minDimension);
}