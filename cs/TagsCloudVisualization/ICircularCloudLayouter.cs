using System.Drawing;

namespace TagsCloudVisualization
{
    public interface ICircularCloudLayouter
    {
        Point Center { get; set; }
        Rectangle PutNextRectangle(Size rectangleSize);
    }
}
