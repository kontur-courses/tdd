using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization.Classes
{
    public class CircularCloudLayouter
    {
        private Point Center { get; set; }
        private SpiralGenerator SpiralGenerator { get; set; }
        public List<Rectangle> Rectangles { get;}
      
        public CircularCloudLayouter(Point center)
        {
            if (center.X < 0 || center.Y < 0)
                throw new ArgumentException("Coordinates of the center must be positive numbers");
            Center = center;
            SpiralGenerator = new SpiralGenerator(Center);
            Rectangles = new List<Rectangle>();
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            var nextRectangle = SpiralGenerator.GetNextRectangleOnSpiral(rectangleSize);

            while (IntersectsWithRectangles(nextRectangle, this.Rectangles))
                nextRectangle = SpiralGenerator.GetNextRectangleOnSpiral(rectangleSize);

            nextRectangle = MoveToCenter(nextRectangle);

            Rectangles.Add(nextRectangle);
            return nextRectangle;
        }

        public List<Rectangle> GenerateTestLayout()
        {
            var rectangleCount = 150;
            var x = 90;
            var y = 10;

            int downSize;

            for (var i = 1; i < rectangleCount; i++)
            {
                downSize = new Random().Next(0,5);
                if (i % 20 == 0)
                    x -= downSize;
                var size = new Size(x, y);
                Rectangles.Add(PutNextRectangle(size));
            }

            return Rectangles;
        }

        public Point GetRectangleCenter(Rectangle rectangle) => rectangle.Location + new Size(rectangle.Width / 2, rectangle.Height / 2);

        public bool IntersectsWithRectangles(Rectangle rectangle, IEnumerable<Rectangle> rectangles) => rectangles.Any(r => r.IntersectsWith(rectangle));

        private Rectangle MoveToCenter(Rectangle rectangle)
        {
            while (true)
            {
                var direction = Center - new Size(GetRectangleCenter(rectangle));
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
