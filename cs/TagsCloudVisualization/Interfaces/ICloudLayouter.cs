using SixLabors.ImageSharp;

namespace TagsCloudVisualization.Interfaces;

public interface ICloudLayouter
{
    public RectangleF PutNextRectangle(SizeF rectangleSize);
}