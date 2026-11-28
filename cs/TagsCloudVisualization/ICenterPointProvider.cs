using System.Drawing;

namespace TagsCloudVisualization;

public interface ICenterPointProvider
{
    public Point GetNextCenterPosition();
}