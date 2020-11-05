using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        private Point Center { get; }
        private ArchimedeanSpiral ArchimedeanSpiral { get; }
        public List<Rectangle> Rectangles { get; }
        private double DistanceBetweenLoops { get; }

        public CircularCloudLayouter(Point center)
        {
            DistanceBetweenLoops = 0.2;
            Center = center;
            ArchimedeanSpiral = new ArchimedeanSpiral(Center, DistanceBetweenLoops);
            Rectangles = new List<Rectangle>();
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Height <= 0 || rectangleSize.Width <= 0)
            {
                throw new ArgumentException("rectangle Height or Size should not be negative or zero");
            }

            return GetNewRectangle(rectangleSize);
        }

        private Rectangle GetNewRectangle(Size rectangleSize)
        {
            var location = ArchimedeanSpiral.GetNextPoint();
            var rectangle = new Rectangle(location, rectangleSize);

            while (Collided(rectangle))
            {
                location = ArchimedeanSpiral.GetNextPoint();
                rectangle = new Rectangle(location, rectangleSize);
            }

            rectangle = MoveCloserToCenter(rectangle);
            Rectangles.Add(rectangle);

            return rectangle;
        }

        private Rectangle MoveCloserToCenter(Rectangle rectangle)
        {
            var movedRectangle = rectangle;

            while (!Collided(rectangle) &&
                   rectangle.X != Center.X &&
                   rectangle.Y != Center.Y)
            {
                movedRectangle = rectangle;
                var deltaX = Center.X - rectangle.X < 0 ? -1 : 1;
                var deltaY = Center.Y - rectangle.Y < 0 ? -1 : 1;

                var position = new Point(rectangle.X + deltaX, rectangle.Y + deltaY);
                rectangle = new Rectangle(position, rectangle.Size);
            }

            return movedRectangle;
        }

        private bool Collided(Rectangle newRectangle) =>
            Rectangles.Any(rectangle => rectangle.IntersectsWith(newRectangle));
    }
}