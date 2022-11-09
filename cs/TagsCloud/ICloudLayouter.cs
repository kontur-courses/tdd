using System.Drawing;

namespace TagsCloud
{
    public interface ICloudLayouter<out T>
    {
        T PutNextRectangle(Size size);
    }
}