using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        public Point Center { get; }

        private readonly List<Rectangle> rectangles = new();


        public CircularCloudLayouter(Point center = new())
        {
            Center = center;
        }


        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            var location = GetRectangleLocation(rectangleSize);
            var result = new Rectangle(location, rectangleSize);

            rectangles.Add(result);

            return result;
        }

        public void PutNextRectangles(params Size[] sizes)
        {
            foreach (var s in sizes)
                PutNextRectangle(s);
        }

        private Point GetRectangleLocation(Size rectangleSize)
        {
            if (rectangles.Count == 0)
            {
                var x = (int)Math.Ceiling(Center.X - rectangleSize.Width / 2.0);
                var y = (int)Math.Ceiling(Center.Y + rectangleSize.Height / 2.0);
                return new Point(x, y);
            }

            return new Point(0, 0);
        }

        public IEnumerable<Rectangle> GetRectangles()
            => rectangles;
    }
}