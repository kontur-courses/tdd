using System.Drawing;
using TagsCloudVisualization.Infrastructure.Environment;
using TagsCloudVisualization.Infrastructure.Layout;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        private readonly ILayoutStrategy strategy;
        private readonly Index<Rectangle> index;
        public CircularCloudLayouter(Point center)
        {
            index = new PlainIndex();
            strategy = new SpiralPlacing(new BruteForceCollisionDetector(index), center, 1);
        }
        
        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            var rectangle = new Rectangle(
                strategy.GetPlace(rectangleSize),
                rectangleSize);
            index.Add(rectangle);
            return rectangle;
        }
    }
}