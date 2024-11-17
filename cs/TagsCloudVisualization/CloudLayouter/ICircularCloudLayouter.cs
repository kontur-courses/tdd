using System.Drawing;

namespace TagsCloudVisualization.CloudLayouter;

public interface ICircularCloudLayouter
{
    Rectangle PutNextRectangle(Size rectangleSize);
}