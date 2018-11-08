using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization.Sequences
{
    public interface IPointSequence
    {
        int Step { get; }
        IEnumerable<Point> GetPoints();
    }
}