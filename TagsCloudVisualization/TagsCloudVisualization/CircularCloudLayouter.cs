using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        private Spiral spiral;
        private List<Rectangle> rectangles = new List<Rectangle>();
        public CircularCloudLayouter(Point center)
        {
            spiral = new Spiral(center);
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            foreach (var point in spiral.GetPoints())
            {
                var rectangle = new Rectangle(new Point(point.X - rectangleSize.Width / 2, point.Y - rectangleSize.Height / 2), rectangleSize);
                if (HasOverlappingRectangles(rectangle, rectangles)) continue;
                rectangles.Add(rectangle);
                return rectangle;
            }
            return Rectangle.Empty;
        }
        
        public static bool HasOverlappingRectangles(Rectangle rectangle, IEnumerable<Rectangle> rectangles) =>
            rectangles.Any(r => r.IntersectsWith(rectangle));
        
        //различная реализация из-за сложностей алгоритмов 
        public static bool HasOverlappingRectangles( IEnumerable<Rectangle> rectangles) =>
             rectangles.Any(r => rectangles.Any(r1 => r != r1 && r.IntersectsWith(r1)));
    }
}