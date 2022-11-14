using System.Drawing;

namespace TagsCloudVisualization
{
    public interface IRectangleLayouter
    {
        public Rectangle PutNextRectangle(Size rectangleSize);
    }
}