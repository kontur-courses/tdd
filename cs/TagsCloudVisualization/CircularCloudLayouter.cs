using System.Drawing;
using TagsCloudVisualization.Extensions;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        private readonly Point center;
        private readonly List<Rectangle> rectangles;
        public List<Rectangle> Rectangles => rectangles.ToList();

        private double angle;
        private double radius;
        private const double AngleStep = Math.PI / 12;
        private const double RadiusStep = 0.25;

        public CircularCloudLayouter(Point center)
        {
            this.center = center;
            rectangles = new List<Rectangle>();
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Width == 0 || rectangleSize.Height == 0)
                throw new ArgumentException();

            var rectangle = GetAddedRectangle(rectangleSize, center, angle, radius);
            while (rectangle.IsIntersectsOthersRectangles(rectangles))
            {
                angle += AngleStep;
                radius += RadiusStep;
                rectangle = GetAddedRectangle(rectangleSize, center, angle, radius);
            }

            var movingPoint = GetMovingToPointVector(rectangle, center);

            ShiftRectangleToCenter(ref rectangle, movingPoint);

            rectangles.Add(rectangle);
            return rectangle;
        }


        private void ShiftRectangleToCenter(ref Rectangle rectangle, Point shiftPoint)
        {
            if (shiftPoint.X == 0 && shiftPoint.Y == 0)
                return;

            var resultRectangle = rectangle;
            var stepX = new Point(shiftPoint.X, 0);
            var stepY = new Point(0, shiftPoint.Y);
            while ((IsCanMove(resultRectangle, stepX) || IsCanMove(resultRectangle, stepY)) && resultRectangle.X != center.X && resultRectangle.Y != center.Y)
            {
                if (IsCanMove(resultRectangle, stepX))
                    resultRectangle = GetMovedRectangle(resultRectangle, stepX);
                if (IsCanMove(resultRectangle, stepY))
                    resultRectangle = GetMovedRectangle(resultRectangle, stepY);
            }
            rectangle = resultRectangle;
        }

        private bool IsCanMove(Rectangle rectangle, Point movePoint)
        {
            var r = GetMovedRectangle(rectangle, movePoint);
            return !r.IsIntersectsOthersRectangles(rectangles);
        }

        private Rectangle GetMovedRectangle(Rectangle rectangle, Point point)
        {
            return new Rectangle(new Point(rectangle.X + point.X, rectangle.Y+point.Y), rectangle.Size);
        }

        private Rectangle GetAddedRectangle(Size rectangleSize, Point centerPoint, double spiralAngle, double spiralRadius)
        {
            var location = new Point(centerPoint.X + GetX(spiralRadius, spiralAngle), centerPoint.Y + GetY(spiralRadius, spiralAngle));
            return new Rectangle(location, rectangleSize);
        }

        private Point GetMovingToPointVector(Rectangle rectangle, Point point)
        {
            var x = (point.X - rectangle.X) == 0 ? 0 : (point.X - rectangle.X) / Math.Abs(point.X - rectangle.X);
            var y = (point.Y - rectangle.Y) == 0 ? 0 : (point.Y - rectangle.Y) / Math.Abs(point.Y - rectangle.Y);
            return new Point(x, y);
        }

        private int GetX(double r, double a) => (int)(r * Math.Cos(a));
        private int GetY(double r, double a) => (int)(r * Math.Sin(a));
    }
}
