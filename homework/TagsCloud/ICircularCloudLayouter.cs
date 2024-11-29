using System.Drawing;

namespace TagsCloud;

public interface ICircularCloudLayouter
{
    public Rectangle PutNextRectangle(Size rectangleSize);
}
