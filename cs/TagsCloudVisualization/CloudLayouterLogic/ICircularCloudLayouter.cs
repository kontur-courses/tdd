using System.Drawing;

namespace TagsCloudVisualization.CloudLayouterLogic
{
    public interface ICircularCloudLayouter
    {
        Rectangle PutNextRectangle(Size rectangleSize);
    }
}