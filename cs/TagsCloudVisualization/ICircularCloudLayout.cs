using System.Drawing;

namespace TagsCloudVisualization
{
    public interface ICircularCloudLayout
    {
        Rectangle PutNextRectangle(Size rectangleSize);
    }
}