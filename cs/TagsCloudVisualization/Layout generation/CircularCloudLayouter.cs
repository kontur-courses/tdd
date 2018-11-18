using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization.Layout_generation
{
    public class CircularCloudLayouter
    {
        private readonly Point center;
        private readonly SpiralGenerator spiralGenerator;
        public List<Rectangle> Rectangles { get; }

        public CircularCloudLayouter(Point center)
        {
            if (center.X < 0 || center.Y < 0)
                throw new ArgumentException("Coordinates of the center must be positive numbers");
            this.center = center;
            spiralGenerator = new SpiralGenerator(this.center);
            Rectangles = new List<Rectangle>();
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            var nextRectangle = new Rectangle(spiralGenerator.GetNextPositionOnSpiral(), rectangleSize);
            while (IntersectsWithRectangles(nextRectangle, this.Rectangles))
                nextRectangle = new Rectangle(spiralGenerator.GetNextPositionOnSpiral(), rectangleSize);
            nextRectangle = MoveToCenter(nextRectangle);
            Rectangles.Add(nextRectangle);
            return nextRectangle;
        }

        public List<Rectangle> GenerateTestLayout()
        {
            var rectangleCount = 150;
            var x = 90;
            var y = 20;

            var random = new Random();
            for (var i = 1; i < rectangleCount; i++)
            {
                if (i % 20 == 0)
                    x -= random.Next(0,5);
                var size = new Size(x, y);
                Rectangles.Add(PutNextRectangle(size));
            }

            return Rectangles;
        }

        public Point GetRectangleCenter(Rectangle rectangle) =>
            rectangle.Location + new Size(rectangle.Width / 2, rectangle.Height / 2);

        public bool IntersectsWithRectangles(Rectangle rectangle, IEnumerable<Rectangle> rectangles) =>
            rectangles.Any(r => r.IntersectsWith(rectangle));

        private Rectangle MoveToCenter(Rectangle rectangle)
        {
            while (true)
            {
                var direction = center - new Size(GetRectangleCenter(rectangle));
                var offsetRectangle = MoveRectangleByOnePoint(rectangle, new Point(Math.Sign(direction.X), 0));
                if (offsetRectangle == rectangle)
                    break;

                offsetRectangle = MoveRectangleByOnePoint(offsetRectangle, new Point(0, Math.Sign(direction.Y)));
                if (offsetRectangle == rectangle)
                    break;
                rectangle = offsetRectangle;
            }
            return rectangle;
        }

        private Rectangle MoveRectangleByOnePoint(Rectangle rectangle, Point offset)
        {
            var offsetRectangle = new Rectangle(rectangle.Location + new Size(offset), rectangle.Size);
            if (IntersectsWithRectangles(offsetRectangle, Rectangles))
                return rectangle;
            return offsetRectangle;
        }
    }
}
