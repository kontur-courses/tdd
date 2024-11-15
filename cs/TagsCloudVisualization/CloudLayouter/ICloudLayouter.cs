using System.Drawing;

namespace TagsCloudVisualization;

public interface ICloudLayouter
{
    public List<Rectangle> GetRectangles();
    public Rectangle PutNextRectangle(Size rectangleSize);
}
