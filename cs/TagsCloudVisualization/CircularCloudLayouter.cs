using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        private readonly Point CenterCoordinates;
        private readonly List<Rectangle> Rectangles;
        private readonly Spiral spiral;

        public CircularCloudLayouter(Point center, Spiral.SpiralDensity density = Spiral.SpiralDensity.dense)
        {
            CenterCoordinates = center;
            Rectangles = new List<Rectangle>();
            spiral = new Spiral(center, density);
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
                throw new ArgumentException("Sides of rectangle can't be negative");
            if (Rectangles.Count == 0)
            {
                var firstRectangle = GetRectangleArrangedInCenter(rectangleSize, CenterCoordinates);
                Rectangles.Add(firstRectangle);
                return firstRectangle;
            }
            var rectangle = new Rectangle(spiral.GetNextPoint(), rectangleSize);
            while (RectangleIntersect(rectangle))
                rectangle = new Rectangle(spiral.GetNextPoint(), rectangleSize);
            Rectangles.Add(rectangle);

            return rectangle;
        }

        private bool RectangleIntersect(Rectangle rectangle)
        {
            return Rectangles.Any(rectangle.IntersectsWith);
        }

        private Rectangle GetRectangleArrangedInCenter(Size rectangleSize, Point center)
        {
            var location = new Point(center.X - rectangleSize.Width / 2,
                center.Y - rectangleSize.Height / 2);
            
            return new Rectangle(location, rectangleSize);
        }
    }
}