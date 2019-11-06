using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        private const int DefaultDistance = 20;

        private readonly Point center;
        private readonly CoordinateGenerator generator;
        private readonly Stack<Rectangle> rectangles;

        public CircularCloudLayouter(Point center) :
            this(center, new ArchimedeanSpiralGenerator(DefaultDistance))
        { }

        public CircularCloudLayouter(Point center, CoordinateGenerator generator)
        {
            this.center = center;
            this.generator = generator;
            rectangles = new Stack<Rectangle>();
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            var shiftPoint = new Point(
                center.X - rectangleSize.Width / 2,
                center.Y - rectangleSize.Height / 2
            );
            foreach (var point in generator.GeneratePoints())
            {
                var location = point.Add(shiftPoint);
                var rectangle = new Rectangle(location, rectangleSize);
                if (rectangles.Any(rectangle.IntersectsWith))
                {
                    continue;
                }
                rectangles.Push(rectangle);
                return rectangle;
            }
            return Rectangle.Empty;
        }
    }
}
