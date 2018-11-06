using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public interface IPointsGenerator
    {
        IEnumerable<Point> GetPoints(int distanceBetweenPoints);
    }
}