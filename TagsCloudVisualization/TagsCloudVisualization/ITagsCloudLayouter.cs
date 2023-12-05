using System.Drawing;

namespace TagsCloudVisualization;

public interface ITagsCloudLayouter
{
    IEnumerable<Rectangle> Rectangles { get; }
    Rectangle PutNextRectangle(Size rectangleSize);
    Point FindPositionForRectangle(Size rectangleSize);
    bool IsPlaceSuitableForRectangle(Rectangle rectangle);
}