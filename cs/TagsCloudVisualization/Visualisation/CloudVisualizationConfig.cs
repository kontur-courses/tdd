using SixLabors.ImageSharp.PixelFormats;
using DrawingRectangle = System.Drawing.Rectangle;
using Point = System.Drawing.Point;

namespace TagCloud.Visualisation;

public class CloudVisualizationConfig(int width, int height,
    Rgba32 backgroundColor, Point center,
    Func<DrawingRectangle, int, Rgba32> rectangleColorProvider)
{
    public int Width { get; } = width;
    public int Height { get; } = height;
    public Rgba32 BackgroundColor { get; } = backgroundColor;
    public Point Center { get; } = center;
    public Func<DrawingRectangle, int, Rgba32> RectangleColorProvider { get; } = rectangleColorProvider;
}