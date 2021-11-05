using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization.Interfaces
{
    public interface IVectorsGenerator
    {
        IEnumerable<Point> Generate();
    }
}