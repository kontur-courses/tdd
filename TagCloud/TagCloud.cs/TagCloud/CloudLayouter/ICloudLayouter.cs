using System.Drawing;

namespace TagCloud
{
    public interface ICloudLayouter
    {
        int Count { get; }

        Rectangle PutNextRectangle(Size size);
    }
}