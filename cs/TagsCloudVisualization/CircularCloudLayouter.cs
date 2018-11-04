using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    class CircularCloudLayouter
    {
        public Size Size { get; set; }
        public Point Center { get; set; }
        public List<Rectangle> Rectangles { get; set; }
        private double SpiralAngle { get; set; }
        public CircularCloudLayouter(Point center)
        {
            Center = center;
            Size = new Size(center.X * 2, center.Y * 2);
            Rectangles = new List<Rectangle>();
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            var rectangle = CreateNewRectangle(rectangleSize);

            while (Rectangles.Any(e => e.IntersectsWith(rectangle)))
            {
                SpiralAngle++;
                rectangle = CreateNewRectangle(rectangleSize);
            }
            Rectangles.Add(rectangle);

            return rectangle;
        }

        private Rectangle CreateNewRectangle(Size rectangleSize)
        {
            var x = Center.X + (int)(SpiralAngle * Math.Cos(SpiralAngle));
            var y = Center.Y + (int)(SpiralAngle * Math.Sin(SpiralAngle));

            var rectangleLocation = new Point(x, y);

            return new Rectangle(rectangleLocation, rectangleSize);
        }
    }
}
