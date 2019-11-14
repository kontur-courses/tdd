using System.Drawing;

namespace TagsCloudVisualization
{
    public interface IRectangleCloudLayouter
    {
        Rectangle PutNextRectangle(Size rectangleSize);
    }
}