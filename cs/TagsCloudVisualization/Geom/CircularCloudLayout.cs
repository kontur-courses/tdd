using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        public List<Rectangle> Rectangles;
        public readonly Spiral Spiral;

        public CircularCloudLayouter(Point center)
        {
            Spiral = new Spiral(center);
            Rectangles = new List<Rectangle>();
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            while (true)
            {
                var location = Spiral.GetNextLocation();
                var rectangle = new Rectangle((int)location.X, (int) location.Y, rectangleSize.Width, rectangleSize.Height);

                if (!IsIntersectedWithOthers(rectangle))
                {
                    Rectangles.Add(rectangle);
                    return rectangle;
                }
            }
        }

        private bool IsIntersectedWithOthers(Rectangle rectangle)
        {
            foreach (var r in Rectangles)
                if (rectangle.IntersectsWith(r))
                    return true;

            return false;
        }
    }
}