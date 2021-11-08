using System;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        private readonly IRectangleLayouter rectangleLayouter;
        
        public CircularCloudLayouter(Point center)
        {
            rectangleLayouter = new SpiralRectangleLayouter(center);
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            return rectangleLayouter.PutNextRectangle(rectangleSize);
        }
    }
}