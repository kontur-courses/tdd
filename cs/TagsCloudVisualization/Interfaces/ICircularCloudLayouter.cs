using System.Drawing;

namespace TagsCloudVisualization.Interfaces
{
    public interface ICircularCloudLayouter
    {
        Rectangle PutNextRectangle(Size rectangleSize);
    }
}