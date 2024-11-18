using System.Drawing;

namespace TagsCloudVisualization
{
    internal class CircularCloudLayouter(Point center) : ICircularCloudLayouter
    {
        private readonly Circle _arrangementСircle = new Circle(center);
        private readonly Random _random = new Random();
        private readonly List<Rectangle> _rectangles = new List<Rectangle>();

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
            {
                throw new ArgumentException("Размеры прямоугольника не могут быть меньше либо равны нуля.");
            }

            var result = new Rectangle();
            _arrangementСircle.Radius -= 1.0f;

            var isPlaced = false;
            while (!isPlaced)
            {
                var startAngle = _random.Next(360);
                foreach (var coordinate in _arrangementСircle.GetCoordinatesOnCircle(startAngle))
                {
                    var location = GetRectangleLocation(coordinate, rectangleSize);
                    var nextRectangle = new Rectangle(location, rectangleSize);
                    if (!IsIntersectionWithAlreadyPlaced(nextRectangle))
                    {
                        _rectangles.Add(nextRectangle);
                        isPlaced = true;
                        result = nextRectangle;
                        break;
                    }
                }

                _arrangementСircle.Radius += 1.0f;
            }

            return result;
        }

        private bool IsIntersectionWithAlreadyPlaced(Rectangle rectangle)
        {
            foreach (var rect in _rectangles)
            {
                if (rect.IntersectsWith(rectangle))
                {
                    return true;
                }
            }

            return false;
        }

        private Point GetRectangleLocation(Point pointOnCircle, Size rectangleSize)
        {
            var x = pointOnCircle.X - rectangleSize.Width / 2;
            var y = pointOnCircle.Y - rectangleSize.Height / 2;
            return new Point(x, y);
        }
    }
}