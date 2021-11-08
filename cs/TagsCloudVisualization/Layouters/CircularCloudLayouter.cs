using System.Drawing;

namespace TagsCloudVisualization.Layouters
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