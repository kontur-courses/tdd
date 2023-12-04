using System.Drawing;

public interface ICloudLayouter
{
    Rectangle PutNextRectangle(Size rectangleSize);
}