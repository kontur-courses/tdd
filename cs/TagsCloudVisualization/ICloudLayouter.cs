using System.Drawing;

namespace TagsCloudVisualization;

public interface ICloudLayouter
{
    List<Rectangle> PlacedRectangles { get; }
    Rectangle PutNextRectangle(Size rectangleSize);
}