using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace CloudTag
{
    public class CircularCloudLayouter
    {
        private readonly Spiral spiral;
        private readonly List<Rectangle> rectangles = new List<Rectangle>();

        public CircularCloudLayouter(Point center)
        {
            spiral = new Spiral(center);
        }
        
        public CircularCloudLayouter(int centerX, int centerY)
        {
            var center = new Point(centerX, centerY);
            spiral = new Spiral(center);
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Height == 0 || rectangleSize.Width == 0)
                return Rectangle.Empty;
            
            var rectangleToAdd = new Rectangle(Point.Empty, rectangleSize);

            do
            {
                rectangleToAdd.SetCenter(spiral.GetNextPoint());
            } while (rectangles.Any(rectangle => rectangle.IntersectsWith(rectangleToAdd)));

            rectangles.Add(rectangleToAdd);
            return rectangles.Last();
        }

        public IEnumerable<Rectangle> GetRectangles()
        {
            return rectangles;
        }
    }
}