using System.Drawing;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter(Point centerPoint)
    {
        public Point CenterPoint { get; } = centerPoint;
        public IReadOnlyList<Rectangle> Rectangles => rectangles.AsReadOnly();
        private readonly List<Rectangle> rectangles = [];
        private readonly ArchimedeanSpiral spiral = new(centerPoint);

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Height <= 0 || rectangleSize.Width <= 0)
                throw new ArgumentException(
                    $"rectangleSize with zero or negative height or width is prohibited!",
                    "rectangleSize"
                );
            while (true)
            {
                var nextPoint = spiral.GetNextPoint();
                var rectangle = new Rectangle(
                    new Point(nextPoint.X - rectangleSize.Width / 2, nextPoint.Y - rectangleSize.Height / 2),
                    rectangleSize
                );
                if (rectangles.Any(x => x.IntersectsWith(rectangle))) continue;
                rectangles.Add(rectangle);
                break;
            }

            return rectangles[^1];
        }
    }
}
