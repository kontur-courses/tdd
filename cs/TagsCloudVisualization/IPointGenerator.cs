using System.Drawing;

namespace TagsCloudVisualization;

public interface IPointGenerator
{
    Point GetNextPoint();

    void Initialise(Point initCenter);
}