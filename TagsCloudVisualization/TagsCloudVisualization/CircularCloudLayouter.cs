using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        private Point center;
        private Spiral spiral;
        private List<Rectangle> rectangles = new List<Rectangle>();
        public CircularCloudLayouter(Point center)
        {
            this.center = center;
            spiral = new Spiral(center);
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            foreach (var point in spiral.GetPoints())
            {
                var rectangle = new Rectangle(new Point(point.X - rectangleSize.Width / 2, point.Y - rectangleSize.Height / 2), rectangleSize);
                if (HasOverlappingRectangles(rectangle)) continue;
                rectangles.Add(rectangle);
                return rectangle;
            }
            return Rectangle.Empty;
        }
        
        private bool HasOverlappingRectangles(Rectangle rectangle) =>
            rectangles.Any(rectangle1 => rectangle1.IntersectsWith(rectangle));
    }
}