using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        public Point Center { get; }
        public List<Rectangle> Rectangles { get; private set; }

        private Spiral spiral;
        
        public CircularCloudLayouter(Point center)
        {
            Rectangles = new List<Rectangle>();
            Center = center;
            spiral = new Spiral(Center, 4, 0.125);
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            var rectangle = new Rectangle();
            do
            {
                rectangle = new Rectangle(spiral.GetNextPoint(), rectangleSize);
            } while (IsRectangleIntersectOther(rectangle));
            Rectangles.Add(rectangle);
            return rectangle;
        }

        private bool IsRectangleIntersectOther(Rectangle rectangle)
        {
            foreach (var otherRectangle in Rectangles)
                if (rectangle.IntersectsWith(otherRectangle))
                    return true;
            
            return false;
        }
    }
}