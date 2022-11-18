using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization.Interfaces
{
    public interface IRectangleLayouter
    {
        public IReadOnlyList<Rectangle> Rectangles { get; }
        public Rectangle PutNextRectangle(Size rectangleSize);
    }
}