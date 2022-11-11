using System.Drawing;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter : ICloudLayouter
    {
        private readonly List<Rectangle> rectangles;
        private readonly Spiral spiral;
        private const int NumberOfPoints = 10_000;

        public CircularCloudLayouter(Spiral spiral)
        {
            if (spiral is null)
                throw new ArgumentException("Spiral should not be null");

            rectangles = new List<Rectangle>();
            this.spiral = spiral;
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Height <= 0 || rectangleSize.Width <= 0)
                throw new ArgumentException("Sides of the rectangle should not be non-positive");

            var points = spiral.GetPoints(NumberOfPoints);
            var point = points[0];

            for (int i = 0; i < points.Count; i++)
            {
                if (!RectangleCanBePlaced(point, rectangleSize))
                    point = points[i];
            }

            var rectangle = new Rectangle(point, rectangleSize);
            rectangles.Add(rectangle);

            return rectangle;
        }

        public List<Rectangle> GetRectangles(List<Size> rectangleSizes)
        {
            foreach (var size in rectangleSizes)
                rectangles.Add(PutNextRectangle(size));

            return rectangles;
        }

        public bool RectangleCanBePlaced(Point position, Size rectangleSize)
        {
            var rect = new Rectangle(position, rectangleSize);
            return !rectangles.Any(rectangle => rectangle.IntersectsWith(rect));
        }
    }
}