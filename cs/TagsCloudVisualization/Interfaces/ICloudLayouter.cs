using System.Drawing;

namespace TagsCloudVisualization.Interfaces;

public interface ICloudLayouter
{
    public Rectangle PutNextRectangle(Size rectangleSize);
    public List<Rectangle> GetRectangles();
    public Point GetCenter();
    public Size GetCloudSize();
}