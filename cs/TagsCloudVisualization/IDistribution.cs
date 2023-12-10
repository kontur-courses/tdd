using System.Drawing;

namespace TagsCloudVisualization
{
    public interface IDistribution
    {
        Point Center { get; }
        Point GetNextPoint();
    }
}
