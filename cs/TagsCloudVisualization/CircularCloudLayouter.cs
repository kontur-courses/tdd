using System.Drawing;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter : ICircularCloudLayouter
    {
        private readonly Point center;
        private readonly List<Rectangle> rectanglesInLayout;
        private readonly Spiral spiral;
        private readonly IEnumerator<Point> pointsOnSpiral;

        public CircularCloudLayouter(Point center)
        {
            this.center = center;
            rectanglesInLayout = new();
            spiral = new(center, 0.05, 0.01);
            pointsOnSpiral = spiral.GetPointsOnSpiral().GetEnumerator();
        }

        public Point CloudCenter { get => center; }
        public IList<Rectangle> Rectangles { get => rectanglesInLayout; }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
            {
                throw new ArgumentException("Rectangle width and height must be positive");
            }

            var currentRectangle = CreateNewRectangle(rectangleSize);

            rectanglesInLayout.Add(currentRectangle);

            return currentRectangle;
        }

        private Rectangle CreateNewRectangle(Size rectangleSize)
        {
            var rectangleLocation = GetLeftUpperCornerFromRectangleCenter(CloudCenter, rectangleSize);
            var rectangle = new Rectangle(rectangleLocation, rectangleSize);

            while (rectanglesInLayout.Any(rect => rect.IntersectsWith(rectangle)))
            {
                pointsOnSpiral.MoveNext();
                rectangleLocation = GetLeftUpperCornerFromRectangleCenter(pointsOnSpiral.Current, rectangleSize);
                rectangle = new Rectangle(rectangleLocation, rectangleSize);
            }

            return rectangle;
        }

        private Point GetLeftUpperCornerFromRectangleCenter(Point rectangleCenter, Size rectangleSize)
        {
            return rectangleCenter - rectangleSize / 2;
        }
    }
}
