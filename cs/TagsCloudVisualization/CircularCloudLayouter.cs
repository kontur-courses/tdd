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
                rectangle = GetNextRectangle(rectangleSize);
            } while (IsRectangleIntersectOther(rectangle));
            Rectangles.Add(rectangle);
            return rectangle;
        }

        private Rectangle GetNextRectangle(Size rectangleSize)
        {
            var position = spiral.GetNextPoint();
            switch (spiral.Quadrant)
            {
                case Quadrant.First:
                    position.Y -= rectangleSize.Height;
                    break;
                case Quadrant.Second:
                    position.X -= rectangleSize.Width;
                    position.Y -= rectangleSize.Height;
                    break;
                case Quadrant.Third:
                    position.X -= rectangleSize.Width;
                    break;
                case Quadrant.Fourth:
                    break;
            }

            return new Rectangle(position, rectangleSize);
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