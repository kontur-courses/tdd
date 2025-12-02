using System.Drawing;

namespace TagsCloudVisualization.Layouters;

public interface ICircularCloudLayouter
{
    IReadOnlyList<Rectangle> Rectangles { get; }
    Rectangle PutNextRectangle(Size rectangleSize);
}