using System.Drawing;

namespace TagsCloudVisualization.LayoutGeneration
{
    public interface ICloudLayouter
    {
        Rectangle PutNextRectangle(Size rectangleSize);
    }
}