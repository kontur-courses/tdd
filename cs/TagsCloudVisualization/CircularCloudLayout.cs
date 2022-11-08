using System.Drawing;

namespace TagsCloudVisualization
{
    public class CircularCloudLayout
    {
        private readonly int radius;
        private readonly List<Point> spiralPoints;
        private List<Rectangle> placedRectangles;

        public CircularCloudLayout(Point center)
        {
            if (center.X < 1)
                throw new ArgumentException("X sould be positive number");
            if (center.Y < 1)
                throw new ArgumentException("Y sould be positive number");
            radius = center.X < center.Y ? center.X : center.Y;
            placedRectangles = new();
            spiralPoints = new SpiralDrawer(center).GetSpiralPoints();
        }

        public bool PutNextRectangle(Size size, out Rectangle rectangle)
        {
            rectangle = new Rectangle();
            if (!ValidateSize(size))
                throw new ArgumentException("Both dimensions must be above zero");
            if (placedRectangles.Count == 0)
                return TryPlaceRectangleInCenter(out rectangle, size);
            foreach (var point in spiralPoints)
            {
                if (PointLiesInRectangles(point))
                    continue;
                rectangle = TryPlaceRectangle(point, size);
                if (rectangle.IsEmpty)
                    continue;
                OffsetRectangle(ref rectangle);
                break;
            }

            if (rectangle.IsEmpty)
                return false;
            placedRectangles.Add(rectangle);
            return true;
        }

        private void OffsetRectangle(ref Rectangle rectangle)
        {
            var canOffsetX = true;
            var canOffsetY = true;
            while (canOffsetY || canOffsetX)
            {
                canOffsetX = rectangle.GetCenter().X > spiralPoints[0].X
                    ? TryOffSet(ref rectangle, -1, 0, (rectangle) => rectangle.GetCenter().X < spiralPoints[0].X)
                    : TryOffSet(ref rectangle, 1, 0, (rectangle) => rectangle.GetCenter().X > spiralPoints[0].X);
                canOffsetY = rectangle.GetCenter().Y > spiralPoints[0].Y
                    ? TryOffSet(ref rectangle, 0, -1, (rectangle) => rectangle.GetCenter().Y < spiralPoints[0].Y)
                    : TryOffSet(ref rectangle, 0, 1, (rectangle) => rectangle.GetCenter().Y > spiralPoints[0].Y);
            }
        }

        private bool TryOffSet(ref Rectangle rectangle, int x, int y, Func<Rectangle, bool> closeToCenter)
        {
            var buffer = rectangle;
            buffer.Offset(x, y);
            if (closeToCenter(buffer))
                return false;
            if (RectangleIntersects(buffer))
                return false;
            rectangle = buffer;
            return true;
        }

        private Rectangle AdjustRectanglePosition(Point point, Size size)
        {
            var x = point.X > spiralPoints[0].X ? point.X : point.X - size.Width;
            var y = point.Y > spiralPoints[0].Y ? point.Y : point.Y - size.Height;
            return new Rectangle(new Point(x, y), size);
        }

        private Rectangle TryPlaceRectangle(Point pointer, Size size)
        {
            var rectangle = AdjustRectanglePosition(pointer, size);
            if (!RectangleIntersects(rectangle) && !RectangleOutOfCircleRange(rectangle))
                return rectangle;
            return new Rectangle();
        }

        private bool TryPlaceRectangleInCenter(out Rectangle rectangle, Size size)
        {
            rectangle = new Rectangle(
                new Point(spiralPoints[0].X - size.Width / 2, spiralPoints[0].Y - size.Height / 2),
                size);
            if (RectangleOutOfCircleRange(rectangle))
                return false;
            placedRectangles.Add(rectangle);
            return true;
        }

        private bool PointLiesInRectangles(Point p) => placedRectangles.Any(x => x.Contains(p));

        private bool RectangleIntersects(Rectangle rectangle) => placedRectangles.Any(x => x.IntersectsWith(rectangle));

        private bool RectangleOutOfCircleRange(Rectangle rectangle)
        {
            var x1 = rectangle.Left - spiralPoints[0].X;
            var y1 = rectangle.Top - spiralPoints[0].Y;
            var x2 = rectangle.Right - spiralPoints[0].X;
            var y2 = rectangle.Bottom - spiralPoints[0].Y;
            return Math.Sqrt(x1 * x1 + y1 * y1) > radius
                   || Math.Sqrt(x2 * x2 + y1 * y1) > radius
                   || Math.Sqrt(x1 * x1 + y2 * y2) > radius
                   || Math.Sqrt(x2 * x2 + y2 * y2) > radius;
        }

        private bool ValidateSize(Size size) => size.Height > 0 && size.Width > 0;
    }
}