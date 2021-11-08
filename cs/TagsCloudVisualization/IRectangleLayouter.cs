using System.Drawing;

namespace TagsCloudVisualization
{
    internal interface IRectangleLayouter
    {
        public Rectangle PutNextRectangle(Size rectangleSize);
    }
}