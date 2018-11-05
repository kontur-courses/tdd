using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        private List<Rectangle> Rectangles;
        private readonly Spiral spiral;
        private readonly Point center = new Point(0,0);

        public CircularCloudLayouter()
        {
            Rectangles = new List<Rectangle>();
            spiral = new Spiral();
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            var rectangle = GenerateNewRectangle(Rectangles, rectangleSize);
            Rectangles.Add(rectangle);
            return rectangle;
        }

        private Rectangle GenerateNewRectangle(List<Rectangle> rectangles, Size rectangleSize)
        {
            Rectangle rectangle;
            if (rectangles.Count == 0)
                return new Rectangle(center.ShiftToLeftRectangleCorner(rectangleSize), rectangleSize);
            while (true)
            {
                var rectangleCenterPointLocation = spiral.GenerateRectangleLocation();
                var rectangleLocation = rectangleCenterPointLocation.ShiftToLeftRectangleCorner(rectangleSize);
                rectangle = new Rectangle(rectangleLocation, rectangleSize);
                if (RectanglesDoNotIntersect(rectangles, rectangle))
                    break;
            }
            return rectangle;
        }

        private bool RectanglesDoNotIntersect(List<Rectangle> rectangles, Rectangle newRectangle)
        {
            return !(rectangles.Any(newRectangle.IntersectsWith));
        }
    }

}
