using System.Drawing;

namespace TagsCloudVisualization
{
    public interface ICloudLayouter<out T>
    {
        T PutNextRectangle(Size size);
    }
}