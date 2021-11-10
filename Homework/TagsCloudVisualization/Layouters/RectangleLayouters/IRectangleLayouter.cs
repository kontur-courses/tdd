using System.Drawing;

namespace TagsCloudVisualization.Layouters.RectangleLayouters
{
    internal interface IRectangleLayouter
    {
        public Rectangle PutNextRectangle(Size rectangleSize);
    }
}