using System.Drawing;

namespace TagCloud;

public interface ICircularCloudLayouter
{
    Point Center { get; }
    IEnumerable<Rectangle> Rectangles { get; }
    Rectangle PutNextRectangle(Size rectangleSize);
}