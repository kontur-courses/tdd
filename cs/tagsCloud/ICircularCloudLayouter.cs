using System.Drawing;

namespace tagsCloud;

public interface ICircularCloudLayouter
{
    List<Rectangle> Rectangles { get; }
    Rectangle PutNextRectangle(Size rectangleSize);
    List<Point> GetRectanglesLocation();
}