using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        public Point Center { get; }
        public List<Rectangle> Rectangles { get; }
        private readonly PointProvider point;
        

        public CircularCloudLayouter(Point point)
        {
            if (point.X < 0 || point.Y < 0)
                throw new ArgumentException("X or Y of center was negative");

            Center = point;
            Rectangles = new List<Rectangle>();
            this.point = new PointProvider(Center);
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Height < 0 || rectangleSize.Width < 0)
                throw new ArgumentException("Width or height of rectangle was negative");

            var rectangle = GetRectangle(rectangleSize, Rectangles);
            Rectangles.Add(rectangle);
            
            return rectangle;
        }

        private Rectangle GetRectangle(Size rectangleSize, List<Rectangle> rectangles)
        {
            Rectangle rectangle;
            do
            {
                rectangle = new Rectangle(point.GetPoint() 
                                          - new Size(rectangleSize.Width / 2, rectangleSize.Height / 2), rectangleSize);

            } while (IsCollide(rectangles, rectangle));

            return rectangle;
        }

        private static bool IsCollide(List<Rectangle> rectangles, Rectangle rectangle)
        {
            return rectangles.Where(rectangle.IntersectsWith).Any();
        }
    }
}

