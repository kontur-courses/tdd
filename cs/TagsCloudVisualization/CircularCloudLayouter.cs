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

        public CircularCloudLayouter(Point center)
        {
            Cloud = new Cloud(center);
            spiralEnumerator = SpiralGenerator.GetSpiral(center, 5, Math.PI / 16).GetEnumerator();
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            var corner = GetUpperLeftCorner(rectangleSize);
            var rectangle = new Rectangle(corner, rectangleSize);

            Cloud.Rectangles.Add(rectangle);

            return rectangle;
        }

        private Point GetUpperLeftCorner(Size rectangleSize)
        {
            while (true)
            {
                if (TryGetNextLocation(rectangleSize, out var location))
                {
                    return location;
                }
            }
        }

        private bool TryGetNextLocation(Size rectangleSize, out Point location)
        {
            spiralEnumerator.MoveNext();
            location = spiralEnumerator.Current;
            var rectangle = new Rectangle(location, rectangleSize);

            return Cloud.Rectangles.All(r => !r.IntersectsWith(rectangle));
        }
    }
}