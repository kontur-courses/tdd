using System;
using System.Drawing;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("TagsCloudVisualization_Tests")]
namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        public Point Center { get; }
        internal List<Rectangle> Rectangles { get; private set; }

        private Spiral spiral;
        
        public CircularCloudLayouter(Point center)
        {
            Rectangles = new List<Rectangle>();
            Center = center;
            spiral = new Spiral(Center, 4, 0.005);
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            Rectangle rectangle;
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
                case Quadrant.BottomRight:
                    position.Y -= rectangleSize.Height;
                    break;
                case Quadrant.BottomLeft:
                    position.X -= rectangleSize.Width;
                    position.Y -= rectangleSize.Height;
                    break;
                case Quadrant.TopLeft:
                    position.X -= rectangleSize.Width;
                    break;
                case Quadrant.TopRight:
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