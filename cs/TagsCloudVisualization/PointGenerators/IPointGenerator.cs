using System.Drawing;

namespace TagsCloudVisualization.PointGenerators;

public interface IPointGenerator
{
    public Point GetPoint();
}