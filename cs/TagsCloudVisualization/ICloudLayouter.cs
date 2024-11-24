using System.Drawing;

namespace TagsCloudVisualization;

public interface ICloudLayouter
{
    RectangleF PutNextRectangle(SizeF rectangleSize);
}