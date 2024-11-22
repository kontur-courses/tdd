using SkiaSharp;

namespace TagsCloudVisualization.Layouter;

public interface ICircularCloudLayouter
{
    SKRect PutNextRectangle(SKSize rectangleSize);
    SKRect[] GetRectangles();
}