using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public interface IPointGenerator
    {
        IEnumerable<PointF> GetPoints(Size tagSize);
    }
}