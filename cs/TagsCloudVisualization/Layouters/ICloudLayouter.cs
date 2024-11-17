using System.Drawing;

namespace TagsCloudVisualization.Layouters;

public interface ICloudLayouter
{
    Rectangle PutNextRectangle(Size rectangleSize);
}