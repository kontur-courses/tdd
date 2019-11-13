using System.Drawing;

namespace TagsCloudVisualization
{
    public interface RectangleCloudLayouter
    {
        Rectangle PutNextRectangle(Size rectangleSize);
    }
}