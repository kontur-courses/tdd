using System.Collections.Immutable;
using System.Drawing;

namespace TagCloud;

public interface ICloudLayouter
{
    public ImmutableArray<Rectangle> Rectangles { get; }
    public Rectangle PutNextRectangle(Size rectangleSize);
}