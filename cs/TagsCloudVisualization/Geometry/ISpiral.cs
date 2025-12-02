using System.Drawing;

namespace TagsCloudVisualization.Geometry;

public interface ISpiral
{
    Point Center { get; }
    Point GetNextPoint();
}