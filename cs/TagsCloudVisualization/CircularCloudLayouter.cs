using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    class CircularCloudLayouter
    {
        public readonly Point Center;
        private List<Rectangle> Rectangles { get; }
        private readonly SpiralGenerator generator;

        public CircularCloudLayouter(Point center)
        {
            if (center.X < 0 || center.Y < 0)
                throw new ArgumentException();
            Center = center;
            Rectangles = new List<Rectangle>();
            generator = new SpiralGenerator(Center);
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Height <= 0 || rectangleSize.Width <= 0)
                throw new ArgumentException();
            var newRectangle = PutRectangleOnSpiral(rectangleSize);
            while (Rectangles.Any(x => x.IntersectsWith(newRectangle)))
            {
                newRectangle = PutRectangleOnSpiral(rectangleSize);
            }

            newRectangle = MoveToCenter(newRectangle);
            Rectangles.Add(newRectangle);
            return newRectangle;
        }

        private Rectangle PutRectangleOnSpiral(Size rectangleSize)
        {
            var point = generator.GetNextPoint();
            return new Rectangle(new Point(point.X - rectangleSize.Width / 2, point.Y - rectangleSize.Height / 2), rectangleSize);
        }

        private Rectangle MoveToCenter(Rectangle rectangle)
        {
            while (true)
            {
                var direction = Center - (Size)rectangle.GetCenter();

                var offsetRectangle = new Rectangle(rectangle.Location + new Size(Math.Sign(direction.X), 0), rectangle.Size);
                if (Rectangles.Any(el => el.IntersectsWith(offsetRectangle)) || offsetRectangle.GetCenter() == Center)
                    break;

                rectangle = offsetRectangle;

                offsetRectangle = new Rectangle(rectangle.Location + new Size(0, Math.Sign(direction.Y)), rectangle.Size);
                if (Rectangles.Any(el => el.IntersectsWith(offsetRectangle)))
                    break;

                rectangle = offsetRectangle;
            }

            return rectangle;
        }
    }
}
