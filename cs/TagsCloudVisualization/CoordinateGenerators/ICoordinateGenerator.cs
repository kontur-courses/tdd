using System.Drawing;

namespace TagsCloudVisualization.CoordinateGenerators;

public interface ICoordinateGenerator
{
    Point GetNextPosition();
}