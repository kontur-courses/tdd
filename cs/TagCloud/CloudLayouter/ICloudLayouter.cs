using System.Drawing;

public interface ICloudLayouter
{
    Point Center { get; }
    int Count { get; }

    Rectangle PutNextRectangle(Size size);
}