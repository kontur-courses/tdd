using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    internal class CircularCloudLayouter
    {
        private readonly Point center;
        private readonly List<Rectangle> rectangles = new List<Rectangle>();

        public List<Rectangle> Rectangles => rectangles.Select(r => new Rectangle(r.Location, r.Size)).ToList();

        public CircularCloudLayouter(Point center)
        {
            this.center = center;
        }

        public Rectangle PutNextRectangle(Size size)
        {
            var rectangle = GenerateSuitableRectangle(size);
            rectangles.Add(rectangle);
            return rectangle;
        }

        private Rectangle GenerateSuitableRectangle(Size size)
        {
            var rectangle = new Rectangle(center, size);

            if (rectangles.Any())
                FindSuitablePlaceFor(ref rectangle);
            else
            {
                rectangle.X -= size.Width / 2;
                rectangle.Y -= size.Height / 2;
            }

            return rectangle;
        }

        private void FindSuitablePlaceFor(ref Rectangle rectangle)
        {
            foreach (var direction in SpiralPath.Clockwise(Direction.Down))
            {
                rectangle = rectangle.Moved(direction);
                if (!IntersectsWithExistingRectangles(rectangle))
                    break;
            }
        }

        private bool IntersectsWithExistingRectangles(Rectangle rect)
        {
            return rectangles.Any(r => r.IntersectsWith(rect));
        }
    }
}
