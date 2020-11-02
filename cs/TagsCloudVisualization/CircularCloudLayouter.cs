using System.Drawing;
using TagsCloudVisualization.Infrastructure.Environment;
using TagsCloudVisualization.Infrastructure.Layout;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        private readonly ILayoutStrategy strategy;
        private readonly Environment<Rectangle> environment;

        public CircularCloudLayouter(Point center)
        {
            strategy = new SpiralPlacing(center, 1);
            environment = new PlainEnvironment();
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            var rectangle = new Rectangle(Point.Empty, rectangleSize);
            var rectangleMiddleOffset = new Size(rectangle.Width / 2, rectangle.Height / 2);

            bool IsValid(Point possiblePoint)
            {
                rectangle.Location = possiblePoint - rectangleMiddleOffset;
                var isValid = !environment.IsColliding(rectangle);
                return isValid;
            }

            var location = strategy.GetPlace(IsValid);
            rectangle.Location = location - rectangleMiddleOffset;
            
            environment.Add(rectangle);
            return rectangle;
        }
    }
}