using System.Drawing;

namespace TagsCloudVisualization
{
    public interface IPointDistributor
    {
        Point GetPosition(Cloud cloud, Size rectangleSize, double deltaAngle);
    }
}