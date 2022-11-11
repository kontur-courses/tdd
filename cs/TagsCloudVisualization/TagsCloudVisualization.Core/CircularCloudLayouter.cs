using System.Drawing;
using TagsCloudVisualization.Core.Extensions;

namespace TagsCloudVisualization.Core
{
    public class CircularCloudLayouter
    {
        public readonly Point Center;       
        private readonly ArchimedeanSpiral _spiral;

        public List<Rectangle> Rectangles { get; }

        public CircularCloudLayouter(Point center)
        {
            if (center.X < 0 || center.Y < 0)
                throw new ArgumentException("X or Y is negative!");

            Center = center;
            Rectangles = new List<Rectangle>();
            _spiral = new ArchimedeanSpiral(center);
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Height <= 0 || rectangleSize.Width <= 0)
                throw new ArgumentException("X or Y is negative!");

            var rectangle = GetCorrectRectangle();

            rectangle = ShiftRectangleToCenter(rectangle);
            Rectangles.Add(rectangle);

            return rectangle;

            Rectangle GetCorrectRectangle()
            {
                var rect = GetNextRectangle(rectangleSize);

                while (rect.IntersectsWith(Rectangles))
                    rect = GetNextRectangle(rectangleSize);
                
                return rect;
            }
        }

        private Rectangle GetNextRectangle(Size rectangleSize)
        {
            var point = _spiral.GetNextPoint();
            return new Rectangle(new Point(point.X - rectangleSize.Width / 2,
                                           point.Y - rectangleSize.Height / 2), 
                                           rectangleSize);
        }

        private Rectangle ShiftRectangleToCenter(Rectangle rectangle)
        {
            while (true)
            {
                var direction = Center - (Size)rectangle.GetCenter();

                var newRectangle = ShiftRectangle(rectangle, new Point(Math.Sign(direction.X), 0));
                newRectangle = ShiftRectangle(newRectangle, new Point(0, Math.Sign(direction.Y)));

                if (newRectangle == rectangle)
                    break;
                
                rectangle = newRectangle;
            }

            return rectangle;
        }

        private Rectangle ShiftRectangle(Rectangle rectangle, Point direction)
        {
            var newRectangle = new Rectangle(rectangle.Location, rectangle.Size);
            newRectangle.Offset(direction);

            return newRectangle.IntersectsWith(Rectangles) ? rectangle : newRectangle;
        }
    }
}