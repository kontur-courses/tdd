using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization.Geom
{
    public class CircularCloudLayouter
    {
        private List<Rectangle> rectangles;
        private Spiral spiral;

        public IReadOnlyCollection<Rectangle> Rectangles => new ReadOnlyCollection<Rectangle>(rectangles);
        public Point Center => spiral.Center;

        public CircularCloudLayouter(Point center)
        {
            spiral = new Spiral(center);
            rectangles = new List<Rectangle>();
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            while (true)
            {
                var location = spiral.GetNextLocation();
                var rectangle = new Rectangle((int)location.X, (int) location.Y, rectangleSize.Width, rectangleSize.Height);

                if (!rectangles.Any(rectangle.IntersectsWith))
                {
                    rectangles.Add(rectangle);
                    return rectangle;
                }
            }
        }
    }
}