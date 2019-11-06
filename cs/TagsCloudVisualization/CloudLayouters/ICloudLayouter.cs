using System.Drawing;

namespace TagsCloudVisualization.CloudLayouters
{
    public interface ICloudLayouter
    {
        Rectangle PutNextRectangle(Size rectangleSize);
    }
}