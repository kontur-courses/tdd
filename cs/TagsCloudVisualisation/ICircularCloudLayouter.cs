using System.Drawing;

namespace TagsCloudVisualisation
{
    public interface ICircularCloudLayouter
    {
        Rectangle PutNextRectangle(Size rectangleSize);
    }
}