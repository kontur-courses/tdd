using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        public readonly Point CenterCoordinates;
        public List<Rectangle> Recatangles { get; private set; }
        private readonly Spiral spiral;

        public CircularCloudLayouter(Point center)
        {
            CenterCoordinates = center;
            Recatangles = new List<Rectangle>();
            spiral = new Spiral(center);
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (Recatangles.Count == 0)
            {
                var firstRectangle = GetFirstRectangle(rectangleSize);
                Recatangles.Add(firstRectangle);
                return firstRectangle;
            }

            var rectangle = new Rectangle(spiral.GetNextPoint(), rectangleSize);
            while (rectangleIntersect(rectangle))
            {
                rectangle = new Rectangle(spiral.GetNextPoint(), rectangleSize);
            }

            Recatangles.Add(rectangle);

            return rectangle;
        }

        private bool rectangleIntersect(Rectangle rectangle)
        {
            return Recatangles.Any(validRectangle => rectangle.IntersectsWith(validRectangle));
        }

        private Rectangle GetFirstRectangle(Size rectangleSize)
        {
            var location = new Point(CenterCoordinates.X - rectangleSize.Width / 2,
                CenterCoordinates.Y - rectangleSize.Height / 2);
            return new Rectangle(location, rectangleSize);
        }
    }
}