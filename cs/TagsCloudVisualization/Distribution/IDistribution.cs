using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public interface IDistribution
    {
        IEnumerable<Point> GetPoints();
    }
}