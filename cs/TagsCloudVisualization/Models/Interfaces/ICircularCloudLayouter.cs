using System.Drawing;

namespace TagsCloudVisualization.Models.Interfaces
{
    public interface ICircularCloudLayouter
    {
        Rectangle PutNextRectangle(Size rectangleSize);
    }
}
