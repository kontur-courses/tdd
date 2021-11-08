using System;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        private readonly ILayouter layouter;
        
        public CircularCloudLayouter(Point center)
        {
            layouter = new SpiralLayouter(center);
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            throw new NotImplementedException();
        }
    }
}