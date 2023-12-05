using System.Drawing;

namespace TagsCloudVizualization;

public interface ICircularCloudLayouter
{
    public Point CloudCenter { get; }
    public IList<Rectangle> Rectangles { get; }
    Rectangle PutNextRectangle(Size rectangleSize);
}