using System.Drawing;

namespace TagsCloudVisualization.Interfaces;

public interface ICloudLayouter
{
    Rectangle PutNextRectangle(Size rectangleSize);
    ICollection<Rectangle> Rectangles();
    Point Center();
}