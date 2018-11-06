using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        public readonly Point Center;
        public readonly List<Rectangle> Rectangles = new List<Rectangle>();

        public CircularCloudLayouter(Point center)
        {
            Center = center;
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            var location = LocateNextRectangle(rectangleSize);
            var rectangle = new Rectangle(location, rectangleSize);
            Rectangles.Add(rectangle);
            return rectangle;
        }

        private Point LocateNextRectangle(Size rectangleSize)
        {
            var theta = 0.0;
            const double thetaIncrement = 0.01;
            while (true)
            {
                var x = Center.X + theta * Math.Cos(theta);
                var y = Center.Y + theta * Math.Sin(theta);
                var location = new Point((int)x, (int)y);
                var rectangle = new Rectangle(location, rectangleSize);
                theta += thetaIncrement;
                if (Rectangles.Any(rectangle.IntersectsWith))
                    continue;
                return location;
            }
        }
    }
}
