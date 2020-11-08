using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization.TagCloud
{
    public interface ICloudLayouter
    {
        public Point Center { get; }
        public IEnumerable<Rectangle> GetAddedRectangles();
        public Rectangle PutNextRectangle(Size rectangleSize);
    }
}