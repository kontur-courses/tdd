using System.Drawing;

namespace TagsCloudVisualization.Layouter;

public interface ICloudLayouter
{
    Rectangle PutNextRectangle(Size rectangleSize);
    IEnumerable<Rectangle> GetRectanglesLayout();
    void ClearRectanglesLayout();
}