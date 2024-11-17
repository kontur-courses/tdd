using System.Drawing;

namespace TagsCloudVisualization.PointsGenerators;

public interface IPointsGenerator
{
    Point GetNextPointPosition();
}
