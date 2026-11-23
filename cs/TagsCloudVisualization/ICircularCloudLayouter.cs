using System.Drawing;

namespace TagsCloudVisualization;

public interface ICircularCloudLayouter
{
    Rectangle PutNextRectangle(Size rectangleSize);
    IReadOnlyList<Rectangle> Rectangles { get; }
}