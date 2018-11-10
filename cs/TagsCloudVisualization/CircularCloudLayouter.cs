using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        public Size Size { get; }
        public Point Center { get; }
        private readonly List<Rectangle> rectangles;
        private readonly Spiral spiral;
        
        public CircularCloudLayouter(Point center)
        {
            Center = center;
            Size = new Size(center.X * 2, center.Y * 2);
            rectangles = new List<Rectangle>();
            spiral = new Spiral(Center);
        }

        public ReadOnlyCollection<Rectangle> GetRectangles() => rectangles.AsReadOnly();
        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Height < 1 || rectangleSize.Width < 1)
                throw new ArgumentException("Размер прямоугольника должен быть больше 0");

            var rectangle = CreateNewRectangle(rectangleSize);

            while (rectangles.Any(e => e.IntersectsWith(rectangle)))
            {
                rectangle = CreateNewRectangle(rectangleSize);
            }
            rectangle = MoveRectangleCloserToCenter(rectangle);
            rectangles.Add(rectangle);

            return rectangle;
        }

        private Rectangle MoveRectangleCloserToCenter(Rectangle rectangle)
        {
            rectangle = MoveRectangleCloserToCenterByY(rectangle);
            return MoveRectangleCloserToCenterByX(rectangle);
        }
        private Rectangle MoveRectangleCloserToCenterByY(Rectangle rectangle)
        {
            var rect = rectangle;
            while (!rectangles.Any(e => e.IntersectsWith(rect)))
            {
                if (rect.Y > Center.Y)
                    rect.Y--;

                if (rect.Y < Center.Y)
                    rect.Y++;

                if(!rectangles.Any(e => e.IntersectsWith(rect)))
                    rectangle = rect;

                if (rectangle.Y == Center.Y)
                    break;
            }
            return rectangle;
        }
        private Rectangle MoveRectangleCloserToCenterByX(Rectangle rectangle)
        {
            var rect = rectangle;
            while (!rectangles.Any(e => e.IntersectsWith(rect)))
            {
                if (rect.X > Center.X)
                    rect.X--;

                if (rect.X < Center.X)
                    rect.X++;

                if(!rectangles.Any(e => e.IntersectsWith(rect)))
                    rectangle = rect;

                if (rectangle.X == Center.X)
                    break;
            }
            return rectangle;
        }

        private Rectangle CreateNewRectangle(Size rectangleSize)
        {
            var rectangleLocation = spiral.GetNextPoint();

            return new Rectangle(rectangleLocation, rectangleSize);
        }
    }
}
