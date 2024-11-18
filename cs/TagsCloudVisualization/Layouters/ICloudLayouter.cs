using System.Drawing;

namespace TagsCloudVisualization.Layouters;

public interface ICloudLayouter
{
    public Rectangle PutNextRectangle(Size rectangleSize);
}