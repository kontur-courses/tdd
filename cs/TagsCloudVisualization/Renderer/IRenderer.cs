using SkiaSharp;

namespace TagsCloudVisualization.Renderer;

public interface IRenderer
{
    void DrawRectangles(IEnumerable<SKRect> rectangles);
    
    SKData GetEncodedImage();

}