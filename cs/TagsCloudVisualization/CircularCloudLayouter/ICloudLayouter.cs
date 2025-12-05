using System.Drawing;

namespace TagsCloudVisualization.CircularCloudLayouter;

public interface ICloudLayouter
{
    Rectangle PutNextRectangle(Size rectangleSize);
}