using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public interface IPointsGenerator
    {
        Point GetNextPoint();
        List<Point> AllGeneratedPoints { get; }
    }
}