using System.Drawing;

namespace TagCloud
{
    public class CircularCloudLayouter : ICircularCloudLayouter
    {
        private readonly Point center;
        private readonly SpiralGenerator spiral;
        private IList<Rectangle> rectangles;
        public IList<Rectangle> Rectangles => rectangles.ToList().AsReadOnly();

        public CircularCloudLayouter(Point center)
        {
            this.center = center;
            rectangles = new List<Rectangle>();
            spiral = new SpiralGenerator(center);
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Height == 0 || rectangleSize.Width == 0)
            {
                throw new ArgumentException();
            }

            var rectangle = new Rectangle(spiral.GetNextPoint(), rectangleSize);
            while (!CanPlaceRectangle(rectangle))
            {
                rectangle = new Rectangle(spiral.GetNextPoint(), rectangleSize);
            }

            rectangles.Add(rectangle);
            return rectangle;
        }

        private bool CanPlaceRectangle(Rectangle newRectangle)
        {
            return !rectangles.Any(rectangle => rectangle.IntersectsWith(newRectangle));
        }
    }
}