using System.Drawing;

namespace TagsCloudVisualization;

public interface IPointDistributor
{
    PointF GetNextPoint();
}