using System.Drawing;

namespace TagsCloudVisualization;

public interface ICloudLayouter
{
    public Rectangle PutNextRectangle(Size rectangleSize);
}