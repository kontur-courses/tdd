using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        private readonly Point center;
        private readonly List<Rectangle> rectangles = new List<Rectangle>();
        public Rectangle[] Rectangles => rectangles.ToArray();

        public CircularCloudLayouter(Point center)
        {
            this.center = center;
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            var location = FindPositionForNextRectangle(rectangleSize);
            var rectangle = new Rectangle(location, rectangleSize);
            rectangles.Add(rectangle);
            return rectangle;
        }

        private Point FindPositionForNextRectangle(Size rectangleSize)
        {
            var theta = 0.0;
            const double thetaIncrement = 0.01;
            while (true)
            {
                var x = center.X + theta * Math.Cos(theta);
                var y = center.Y + theta * Math.Sin(theta);
                var location = new Point((int)x, (int)y);
                var rectangle = new Rectangle(location, rectangleSize);
                theta += thetaIncrement;
                if (rectangles.Any(rectangle.IntersectsWith))
                    continue;
                return location;
            }
        }
    }
}
