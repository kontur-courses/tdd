using System.Drawing;

public interface ICloudLayouter
{
    Rectangle PutNextRectangle(Size rectangleSize);
    List<Rectangle> GetLayout();
}