using System.Drawing;

namespace TagsCloudVisualization
{
    internal interface ILayouter
    {
        public Rectangle PutNextRectangle(Size rectangleSize);
    }
}