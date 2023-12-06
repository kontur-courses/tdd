using System.Drawing;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter(Point centerPoint)
    {
        private readonly List<Rectangle> rectangles = new();
        private readonly ArchimedeanSpiral spiral = new(centerPoint);
        public Point CenterPoint { get; } = centerPoint;

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Height <= 0 || rectangleSize.Width <= 0)
                throw new ArgumentException("rectangleSize with zero or negative height or width is prohibited!");
            while (true)
            {
                var nextPoint = spiral.GetNextPoint();
                var rectangle = new Rectangle(new Point(nextPoint.X - rectangleSize.Width / 2, nextPoint.Y -
                    rectangleSize.Height / 2), rectangleSize);
                if (rectangles.Any(x => x.IntersectsWith(rectangle))) continue;
                rectangles.Add(rectangle);
                break;
            }

            return rectangles[^1];
        }

        public IReadOnlyList<Rectangle> GetRectangles() => rectangles.AsReadOnly();
    }
}
