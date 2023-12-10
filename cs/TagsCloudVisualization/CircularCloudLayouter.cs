using System.Drawing;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        private readonly ArchimedeanSpiral spiral;
        private readonly List<Rectangle> rectangles = [];

        public IReadOnlyList<Rectangle> Rectangles => rectangles.AsReadOnly();
        public Point CenterPoint { get; }

        public CircularCloudLayouter(Point centerPoint)
        {
            CenterPoint = centerPoint;
            spiral = new ArchimedeanSpiral(centerPoint);
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Height <= 0 || rectangleSize.Width <= 0)
                throw new ArgumentException(
                    $"rectangleSize with zero or negative height or width is prohibited!",
                    nameof(rectangleSize)
                );
            while (true)
            {
                var nextPoint = spiral.GetNextPoint();
                var newPoint = new Point(nextPoint.X - rectangleSize.Width / 2, nextPoint.Y - rectangleSize.Height / 2);
                var rectangle = new Rectangle(newPoint, rectangleSize);
                if (IsIntersectsWithOthers(rectangle)) continue;
                rectangle = GetCloserToCenterRectangle(rectangle);
                rectangles.Add(rectangle);
                break;
            }

            return rectangles[^1];
        }

        private bool IsIntersectsWithOthers(Rectangle rectangle) =>
            rectangles.Any(x => x.IntersectsWith(rectangle));

        private Rectangle GetCloserToCenterRectangle(Rectangle rectangle)
        {
            var direction = GetDirection(rectangle);
            for (var i = 0; i < direction.Count; i++)
            {
                var newRectangle = GetMovedRectangle(rectangle, direction[i].X, direction[i].Y);
                while (!IsIntersectsWithOthers(newRectangle))
                {
                    if (CenterPoint.X - newRectangle.Size.Width / 2 == newRectangle.X
                        || CenterPoint.Y - newRectangle.Size.Height / 2 == newRectangle.Y)
                        break;
                    rectangle = newRectangle;
                    newRectangle = GetMovedRectangle(rectangle, direction[i].X, direction[i].Y);
                }
            }

            return rectangle;
        }

        private List<(int X, int Y)> GetDirection(Rectangle rectangle)
        {
            var horizontalDiffer = CenterPoint.X - rectangle.Size.Width / 2 - rectangle.X;
            var verticalDiffer = CenterPoint.Y - rectangle.Size.Height / 2 - rectangle.Y;
            var directions = new List<(int X, int Y)>();
            if (horizontalDiffer != 0 && verticalDiffer != 0)
                directions.Add((horizontalDiffer > 0 ? 1 : -1, verticalDiffer > 0 ? 1 : -1));
            if (horizontalDiffer != 0)
                directions.Add((horizontalDiffer > 0 ? 1 : -1, 0));
            if (verticalDiffer != 0)
                directions.Add((0, verticalDiffer > 0 ? 1 : -1));
            return directions;
        }

        private static Rectangle GetMovedRectangle(Rectangle rectangle, int xDelta, int yDelta) =>
            new(
                new Point(
                    rectangle.X + xDelta,
                    rectangle.Y + yDelta
                ),
                rectangle.Size
            );
    }
}
