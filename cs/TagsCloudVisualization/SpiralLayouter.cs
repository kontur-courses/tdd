using System.Drawing;

namespace TagsCloudVisualization
{
    internal class SpiralLayouter : ILayouter
    {
        private readonly Point center;
        
        public SpiralLayouter(Point center)
        {
            this.center = center;
        }
        
        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            throw new System.NotImplementedException();
        }
    }
}