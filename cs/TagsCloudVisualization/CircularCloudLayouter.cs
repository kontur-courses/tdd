using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagsCloudVisualization.tools;

namespace TagsCloudVisualization
{
    internal class CircularCloudLayouter
    {
        public Cloud Cloud { get; }
        private readonly IEnumerator<Point> spiralEnumerator;

        public CircularCloudLayouter(Point center)
        {
            Cloud = new Cloud(center);
            spiralEnumerator = SpiralGenerator.GetSpiral(center, 0.5, Math.PI / 16).GetEnumerator();
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            var position = GetRectangleLocation(rectangleSize);
            var rectangle = new Rectangle(position, rectangleSize);
            Cloud.Rectangles.Add(rectangle);

            return rectangle;
        }

        private Point GetRectangleLocation(Size rectangleSize)
        {
            Point location;

            while (!TryGetNextLocation(rectangleSize, out location))
            {
                //Should be empty
            }

            if (Cloud.Rectangles.Count == 0)
                return location;
            
            location = TryMove(rectangleSize, location, Cloud.Center);
            var previous = Cloud.Rectangles.Last().Location;
           
            return TryMove(rectangleSize, location, previous);
        }

        private bool TryGetNextLocation(Size rectangleSize, out Point upperLeftCorner)
        {
            spiralEnumerator.MoveNext();
            var center = spiralEnumerator.Current;

            upperLeftCorner = GetUpperLeftCornerPosition(rectangleSize, center);
            var rectangle = new Rectangle(upperLeftCorner, rectangleSize);

            return NotIntersectsWithOther(rectangle);
        }
        
        private Point TryMove(Size rectangleSize, Point from, Point to)
        {
            var newLocation = from;
            var nearestToTarget = to;
            var minDistance = Math.Sqrt(2);

            while (newLocation.Distance(nearestToTarget) > minDistance)
            {
                var middle = newLocation.GetMiddlePoint(nearestToTarget);
                var rectangle = new Rectangle(middle, rectangleSize);

                if (NotIntersectsWithOther(rectangle))
                    newLocation = middle;
                else
                    nearestToTarget = middle;
            }

            return newLocation;
        }

        private bool NotIntersectsWithOther(Rectangle rectangle)
        {
            return Cloud.Rectangles.All(r => !r.IntersectsWith(rectangle));
        }

        protected Point GetUpperLeftCornerPosition(Size rectangleSize, Point center)
        {
            var xOffset = rectangleSize.Width / 2;
            var yOffset = rectangleSize.Height / 2;

            return new Point(center.X - xOffset, center.Y - yOffset);
        }
    }
}