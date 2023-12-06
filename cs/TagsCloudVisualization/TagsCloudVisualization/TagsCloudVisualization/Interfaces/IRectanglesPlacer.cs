using System.Drawing;

namespace TagsCloudVisualization.Interfaces;

public interface IRectanglesPlacer
{
    Point Center { get; }
    Point GetNextPoint();
}
