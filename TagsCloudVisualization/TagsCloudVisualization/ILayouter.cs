using System.Drawing;

namespace TagsCloudVisualization;

public interface ILayouter
{
    public IEnumerable<Rectangle> GetLayout();
    public Rectangle PutNextRectangle(Size rectangleSize);
}