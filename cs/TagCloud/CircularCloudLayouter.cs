using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagCloud
{
    public class CircularCloudLayouter
    {
        private readonly Point center;
        public readonly List<Rectangle> Rectangles = new List<Rectangle>();

        public CircularCloudLayouter(Point center)
        {
            this.center = center;
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            var newRect = Rectangles.Count == 0
                ? new Rectangle(center, rectangleSize)
                : FindPlaceForRect(rectangleSize);
            Rectangles.Add(newRect);
            return newRect;
        }

        private Rectangle FindPlaceForRect(Size rectangleSize)
        {
            var spiral = new Spiral(center);
            var resultRect = new Rectangle(spiral.GetNextPoint(), rectangleSize);
            while (Rectangles.Any(rect => rect.IntersectsWith(resultRect)))
                resultRect = new Rectangle(spiral.GetNextPoint(), rectangleSize);

            return resultRect;
        }
    }
}