using System.Drawing;

namespace TagsCloudVisualization.Interfaces;

public interface ICloudDistribution
{
    public Point GetNextPoint();
    public Point GetCenter();
}