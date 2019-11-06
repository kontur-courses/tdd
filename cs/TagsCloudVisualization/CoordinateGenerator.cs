using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public interface CoordinateGenerator
    {
        IEnumerable<Point> GeneratePoints();
    }
}
