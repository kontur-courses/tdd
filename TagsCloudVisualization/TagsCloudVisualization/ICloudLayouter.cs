using System.Drawing;

namespace TagsCloudVisualization
{
    public interface ICloudLayouter
    {
        Rectangle PutNextRectangle(Size rectangleSize);
        Size GetSizeTagCloud();
        Point GetCenter();
    }
}
