using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        public List<Rectangle> Rectangles { get; }
        public Point Center { get; }

        public Spiral Spiral { get; }

        public CircularCloudLayouter(Point center)
        {
            Center = center;
            Rectangles = new List<Rectangle>();
            Spiral = new Spiral(1, 0);
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Height <= 0 || rectangleSize.Width <= 0)
                throw new ArgumentException("Size should be positive");
            var nextRectangle = GenerateNextRectangle(rectangleSize);
            Rectangles.Add(nextRectangle);
            return nextRectangle;
        }


        private Rectangle GenerateNextRectangle(Size rectangleSize)
        {
            if (Rectangles.Any())
            {
                while (true)
                {
                    var rectangleCenter = Spiral.GetNextPoint(Center);
                    var nexRectangle = new Rectangle(rectangleCenter, rectangleSize).ShiftRectangleToBottomLeftCorner();
                    if (!Rectangles.Any(nexRectangle.IntersectsWith))
                        return nexRectangle;
                }
            }
            return new Rectangle(Center, rectangleSize).ShiftRectangleToBottomLeftCorner();
        }
    }
}
