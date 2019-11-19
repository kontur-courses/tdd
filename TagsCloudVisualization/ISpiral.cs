using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public interface ISpiral
    {
        IEnumerable<PointF> GetSpiralPoints();
    }
}