using System.Drawing;

namespace TagsCloudVisualization.Layouters
{
    internal interface IRectangleLayouter
    {
        public Rectangle PutNextRectangle(Size rectangleSize);
    }
}