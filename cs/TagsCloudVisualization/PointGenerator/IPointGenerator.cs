using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization.PointGenerator
{
    public interface IPointGenerator
    {
        IEnumerable<PointF> GetPoints(PointF center);
    }
}