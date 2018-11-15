using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization.Sequences
{
    public interface IPointSequence
    {
        int StepsCount { get; }
        IEnumerable<Point> GetPoints();
    }
}