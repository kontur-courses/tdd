using System.Drawing;

namespace TagsCloudVisualization
{
    public interface IPointGenerator
    {
        PointF GetNextPoint();
    }
}
