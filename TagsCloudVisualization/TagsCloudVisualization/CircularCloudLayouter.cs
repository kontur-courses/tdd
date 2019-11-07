using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        private Point _center;
        private List<Rectangle> _rectangles = new List<Rectangle>();
        private Spiral _spiral;
        public CircularCloudLayouter(Point center)
        {
            this._center = center;
            _spiral = new Spiral(center);
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            foreach (var point in _spiral.GetPoints())
            {
                if (point != null)
                {
                    var rectangle = new Rectangle(new Point(point.Value.X - rectangleSize.Width / 2, point.Value.Y - rectangleSize.Height / 2), rectangleSize);
                    if (IsNonoverlappingRectangle(rectangle))
                    {
                        _rectangles.Add(rectangle);
                        return rectangle;
                    }
                }
            }
            return Rectangle.Empty;
        }
        
        private bool IsNonoverlappingRectangle(Rectangle rectangle)
        {
            return !_rectangles.Any(i => i.IntersectsWith(rectangle));
        }
    }
}