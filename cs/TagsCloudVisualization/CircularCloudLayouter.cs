using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        public readonly Point Center;
        public readonly List<Rectangle> Rectangles;

        private readonly SortedSingleLinkedList<Point> points;
        private Point nextPosition;

        private IEnumerable<Func<Point, Size, Point>> shifts = new List<Func<Point, Size, Point>>
        {
            (p, s) => p,
            (p, s) => new Point(p.X, p.Y - s.Height),
            (p, s) => new Point(p.X - s.Width, p.Y - s.Height),
            (p, s) => new Point(p.X - s.Width, p.Y)
        };

        public CircularCloudLayouter(Point center)
        {
            Center = center;
            Rectangles = new List<Rectangle>();
            points = new SortedSingleLinkedList<Point>(
                (x, y) => center.DistanceTo(x) < center.DistanceTo(y)
            );
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Height <= 0 || rectangleSize.Width <= 0)
                throw new ArgumentException("Size params should be positive");

            var rectangle = Rectangle.Empty;
            if (Rectangles.Count == 0)
            {
                TryPutRectangleToCorners(Center, rectangleSize, out rectangle);
            }
            else
            {
                foreach (var point in points.ToEnumerable())
                    if (TryPutRectangleToCorners(point, rectangleSize, out rectangle))
                        break;
            }
            
            Rectangles.Add(rectangle);
            SaveRectangleBorderPoints(rectangle);
            return rectangle;
        }

        private void SaveRectangleBorderPoints(Rectangle rectangle)
        {
            points.Add(new Point(rectangle.Right, rectangle.Bottom));
            points.Add(new Point(rectangle.Left, rectangle.Bottom));
            points.Add(new Point(rectangle.Right, rectangle.Top));
            points.Add(new Point(rectangle.Left, rectangle.Top));
        }
        
        private bool TryPutRectangleToCorners(Point point, Size rectangleSize, out Rectangle rectangle)
        {
            foreach (var shift in shifts)
            {
                var location = shift(point, rectangleSize);
                rectangle = new Rectangle(location, rectangleSize);
                if (!Rectangles.IntersectsWith(rectangle))
                    return true;
            }

            rectangle = Rectangle.Empty;
            return false;
        }
    }
}