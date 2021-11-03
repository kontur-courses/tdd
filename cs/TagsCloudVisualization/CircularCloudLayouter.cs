using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        public List<Rectangle> Rectangles { get; } = new();

        private readonly Point center;

        public CircularCloudLayouter(Point center)
        {
            this.center = center;
        }

        public void PutNextRectangle(Size rectangleSize)
        {
            var rectangle = CreateAtCenter(rectangleSize);
            while (IsIntersect(rectangle))
            {
                rectangle.X += 1;
            }
            Rectangles.Add(rectangle);
        }

        private Rectangle CreateAtCenter(Size rectangleSize) =>
            new Rectangle(
                new Point(-rectangleSize.Width / 2 + center.X, -rectangleSize.Height / 2 + center.Y),
                rectangleSize);

        private bool IsIntersect(Rectangle rectangle) =>
            Rectangles.Any(other => other.IntersectsWith(rectangle));
    }
}