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
        private Point cloudCenter;
        private ISpiral spiral;
        private List<Rectangle> rectangles;

        public CircularCloudLayouter(Point center)
        {
            cloudCenter = center;
            rectangles = new List<Rectangle>();
            spiral = new Spiral();
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
                throw new ArgumentException("height and width of the rectangle must be greater than 0");

            Point rectangleLocation;

            if (!rectangles.Any())
            {
                rectangleLocation = new Point();
            }
            else
            {
                while (true)
                {
                    rectangleLocation = spiral.GetNextPosition();
                    var newRectangle = new Rectangle(rectangleLocation, rectangleSize);
                    if (!CheckIntersections(newRectangle))
                        break;
                }
                OptimizeRectangleLocation(rectangleLocation, rectangleSize);
            }

            var rectangle = new Rectangle(rectangleLocation, rectangleSize);
            rectangles.Add(rectangle);
            rectangle.Location = new Point(cloudCenter.X + rectangle.Location.X, cloudCenter.Y + rectangle.Location.Y);
            return rectangle;
        }

        private bool CheckIntersections(Rectangle rectangle)
        {
            foreach (var r in rectangles)
            {
                if (r.IntersectsWith(rectangle))
                    return true;
            }

            return false;
        }

        private Point MovePointToCenter(Point point)
        {
            var newX = point.X + Math.Sign(-point.X);
            var newY = point.Y + Math.Sign(-point.Y);
            return new Point(newX, newY);
        }

        private Point OptimizeRectangleLocation(Point point, Size rectangleSize)
        {
            var rectangle = new Rectangle(point,rectangleSize);
            while (true)
            {
                var newLocation = MovePointToCenter(point);
                rectangle.Location = newLocation;
                if (!CheckIntersections(rectangle))
                {
                    point = newLocation;
                }
                else
                    break;
            }

            return point;
        }
    }
}
