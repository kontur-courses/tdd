using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization.Layouters
{
    internal class SpiralRectangleLayouter : IRectangleLayouter
    {
        private readonly Point center;
        private readonly List<Rectangle> rectangles;
        
        public SpiralRectangleLayouter(Point center)
        {
            this.center = center;
        }
        
        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            throw new System.NotImplementedException();
        }
    }
}