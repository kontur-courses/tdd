using System.Drawing;

namespace TagsCloudVisualization
{
    public class CircularCloudLayout
    {
        private readonly int radius;
        private readonly Point center;
        private Point pointer;
        private Size currentSize;
        private Rectangle bufferedRectangle;
        public List<Rectangle> PlacedRectangles { get; }

        public CircularCloudLayout(Point center)
        {
            if (center.X < 1)
                throw new ArgumentException("X sould be positive number");
            if (center.Y < 1)
                throw new ArgumentException("Y sould be positive number");
            this.center = center;
            radius = center.X < center.Y ? center.X : center.Y;
            PlacedRectangles = new();
        }

        public bool PutNextRectangle(Size size, out Rectangle rectangle)
        {
            rectangle = new Rectangle();
            currentSize = size;
            if (!ValidateSize())
                throw new ArgumentException("Both dimensions must be above zero");
            if (PlacedRectangles.Count == 0)
                return TryPlaceRectangleInCenter(out rectangle);
            if (!TryFindPositionByMovePointerInSpiral())
                return false;
            rectangle = bufferedRectangle;
            PlacedRectangles.Add(rectangle);
            return true;
        }

        private bool TryFindPositionByMovePointerInSpiral()
        {
            double angle = 0;
            double adjustAngle = 1;
            while (center.X + Math.Cos(angle) * angle / 0.95 > 0
                   && center.Y + Math.Sin(angle) * angle / 0.95 > 0)
            {
                pointer = new Point((int)(center.X + Math.Cos(angle) * angle / 0.95),
                    (int)(center.Y + Math.Sin(angle) * angle / 0.95));
                var coilCount = (int)(angle / (2 * Math.PI) + 1);
                adjustAngle = adjustAngle <= 0.017 ? 0.017 : Math.PI / 4 / coilCount;
                angle += adjustAngle;
                if (PointLiesInRectangles(pointer))
                    continue;
                if (TryPlaceRectangle())
                {
                    OffsetRectangle();
                    return true;
                }
            }

            return false;
        }

        private void OffsetRectangle()
        {
            var canOffsetX = true;
            var canOffsetY = true;
            while (canOffsetY || canOffsetX)
            {
                canOffsetX = bufferedRectangle.GetCenter().X > center.X
                    ? TryOffSetleft(bufferedRectangle)
                    : TryOffSetRight(bufferedRectangle);
                canOffsetY = bufferedRectangle.GetCenter().Y > center.Y
                    ? TryOffSetUp(bufferedRectangle)
                    : TryOffSetDown(bufferedRectangle);
            }
        }

        private bool TryOffSetleft(Rectangle rect)
        {
            var buffer = rect;
            buffer.Offset(-1, 0);
            if (buffer.GetCenter().X < center.X)
                return false;
            if (RectangleIntesects(buffer))
                return false;
            bufferedRectangle = buffer;
            return true;
        }

        private bool TryOffSetRight(Rectangle rect)
        {
            var buffer = rect;
            buffer.Offset(1, 0);
            if (buffer.GetCenter().X > center.X)
                return false;
            if (RectangleIntesects(buffer))
                return false;
            bufferedRectangle = buffer;
            return true;
        }

        private bool TryOffSetUp(Rectangle rect)
        {
            var buffer = rect;
            buffer.Offset(0, -1);
            if (buffer.GetCenter().Y < center.Y)
                return false;
            if (RectangleIntesects(buffer))
                return false;
            bufferedRectangle = buffer;
            return true;
        }

        private bool TryOffSetDown(Rectangle rect)
        {
            var buffer = rect;
            buffer.Offset(0, 1);
            if (buffer.GetCenter().Y > center.Y)
                return false;
            if (RectangleIntesects(buffer))
                return false;
            bufferedRectangle = buffer;
            return true;
        }

        private Rectangle AdjustRectangle(Point point)
        {
            var x = point.X > center.X ? point.X : point.X - currentSize.Width;
            var y = point.Y > center.Y ? point.Y : point.Y - currentSize.Height;
            return new Rectangle(new Point(x, y), currentSize);
        }

        private bool TryPlaceRectangle()
        {
            bufferedRectangle = AdjustRectangle(pointer);
            return !RectangleIntesects(bufferedRectangle) && !RectangleOutOfCircleRange(bufferedRectangle);
        }

        private bool TryPlaceRectangleInCenter(out Rectangle rectangle)
        {
            rectangle = new Rectangle(new Point(center.X - currentSize.Width / 2, center.Y - currentSize.Height / 2),
                currentSize);
            if (RectangleOutOfCircleRange(rectangle))
                return false;
            PlacedRectangles.Add(rectangle);
            return true;
        }

        private bool PointLiesInRectangles(Point p)
        {
            var result = false;
            PlacedRectangles.ForEach(x => result = result || x.Contains(p));
            return result;
        }

        private bool RectangleIntesects(Rectangle rectangle)
        {
            var result = false;
            PlacedRectangles.ForEach(x => result = result || x.IntersectsWith(rectangle));
            return result;
        }

        private bool RectangleOutOfCircleRange(Rectangle rectangle)
        {
            var x1 = rectangle.X - center.X;
            var y1 = rectangle.Y - center.Y;
            var x2 = rectangle.Right - center.X;
            var y2 = rectangle.Bottom - center.Y;
            return Math.Sqrt(x1 * x1 + y1 * y1) > radius
                   || Math.Sqrt(x2 * x2 + y1 * y1) > radius
                   || Math.Sqrt(x1 * x1 + y2 * y2) > radius
                   || Math.Sqrt(x2 * x2 + y2 * y2) > radius;
        }

        private bool ValidateSize() => currentSize.Height > 0 && currentSize.Width > 0;
    }
}