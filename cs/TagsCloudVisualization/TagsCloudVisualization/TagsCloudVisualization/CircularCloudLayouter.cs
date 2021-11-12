using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter : ICloudLayouter
    {
        public Point Center { get; }
        public List<Rectangle> Rectangles { get; }

        private ISpiral spiral;

        public CircularCloudLayouter(Point center)
        {
            Center = center;
            Rectangles = new List<Rectangle>();
            spiral = new ArchimedeanSpiral(Center, 1, 15);
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
                throw new ArgumentException("height and width of the rectangle must be greater than 0");

            var rectangle = new Rectangle(Center, rectangleSize);

            if (Rectangles.Any())
            {
                while (true)
                {
                    rectangle.Location = spiral.GetNextPosition();
                    if (!CheckIntersections(rectangle))
                        break;
                }
                rectangle = OptimizeRectangleLocation(rectangle);
            }

            Rectangles.Add(rectangle);
            return rectangle;
        }

        private bool CheckIntersections(Rectangle rectangle)
        {
            return Rectangles.Any(rect => rect.IntersectsWith(rectangle));
        }

        private Point MovePointToCenter(Point point)
        {
            var newX = point.X + Math.Sign(Center.X-point.X);
            var newY = point.Y + Math.Sign(Center.Y-point.Y);
            return new Point(newX, newY);
        }

        private Rectangle OptimizeRectangleLocation(Rectangle rectangle)
        {
            var point = rectangle.Location;
            while (true)
            {
                var newLocation = MovePointToCenter(rectangle.Location);
                rectangle.Location = newLocation;
                if (!CheckIntersections(rectangle))
                {
                    point = newLocation;
                }
                else
                    break;
            }

            rectangle.Location = point;
            return rectangle;
        }
    }
}
