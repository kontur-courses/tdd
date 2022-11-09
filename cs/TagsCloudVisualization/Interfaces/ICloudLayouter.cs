using System.Drawing;

namespace TagsCloudVisualization;

public interface ICloudLayouter
{
    Rectangle PutNextRectangle(Size rectangleSize);
    ICollection<Rectangle> Rectangles();
    Point Center();
}