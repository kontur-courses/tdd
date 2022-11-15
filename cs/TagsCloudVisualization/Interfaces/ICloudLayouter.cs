using System.Drawing;

namespace TagsCloudVisualization.Interfaces;

public interface ICloudLayouter
{
    Rectangle PutNextRectangle(Size rectangleSize);
    IReadOnlyCollection<Rectangle> Rectangles();
    Point Center();
}