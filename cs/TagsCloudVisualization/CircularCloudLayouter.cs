using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    internal class CircularCloudLayouter
    {
        public Cloud Cloud { get; }
        private readonly IEnumerator<Point> spiralEnumerator;
        private Size currentRectangleSize;

        public CircularCloudLayouter(Point center)
        {
            Cloud = new Cloud(center);
            spiralEnumerator = SpiralGenerator.GetSpiral(center, 5, Math.PI / 16).GetEnumerator();
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            currentRectangleSize = rectangleSize;

            var position = GetRectanglePosition();
            var rectangle = new Rectangle(position, rectangleSize);
            Cloud.Rectangles.Add(rectangle);

            return rectangle;
        }

        private Point GetRectanglePosition()
        {
            Point location;

            while (!TryGetNextLocation(out location))
            {
                //Should be empty
            }

            return location;
        }

        private bool TryGetNextLocation(out Point location)
        {
            spiralEnumerator.MoveNext();
            var upperLeftCorner = spiralEnumerator.Current;
            
            location = GetCenterPosition(upperLeftCorner);
            var rectangle = new Rectangle(location, currentRectangleSize);

            return Cloud.Rectangles.All(r => !r.IntersectsWith(rectangle));
        }

        private Point GetCenterPosition(Point corner)
        {
            var xOffset = currentRectangleSize.Width / 2;
            var yOffset = currentRectangleSize.Height / 2;

            return new Point(corner.X - xOffset, corner.Y - yOffset);
        }
    }
}