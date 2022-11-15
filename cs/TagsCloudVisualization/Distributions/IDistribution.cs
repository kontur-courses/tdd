using System.Drawing;

namespace TagsCloudVisualization.Distributions
{
    public interface IDistribution
    {
        public Point GetNextPoint();
    }
}