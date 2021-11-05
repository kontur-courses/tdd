using System.Collections.Generic;
using System.Drawing;

namespace TagsCloud.Visualization
{
    public interface IPointGenerator
    {
        IEnumerable<Point> GenerateNextPoint();
    }
}