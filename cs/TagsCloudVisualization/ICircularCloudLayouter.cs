using System.Drawing;

namespace TagsCloudVisualization
{
    public interface ICircularCloudLayouter
    {
        public Point CloudCenter { get; }
        Rectangle PutNextRectangle(Size rectangleSize);
    }
}
