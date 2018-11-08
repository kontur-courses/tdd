using System.Drawing;

namespace TagsCloudVisualization.CloudLayouts
{
    public interface ICloudLayout
    {
        Rectangle PutNextRectangle(Size rectangleSize);
    }
}