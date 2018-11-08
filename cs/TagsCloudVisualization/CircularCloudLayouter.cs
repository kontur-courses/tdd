using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Collections.Immutable;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        private readonly List<Rectangle> rectangles;

        public Point Center { get; }

        private Spiral Spiral { get; }

        public int Radius => GetRadius();

        public CircularCloudLayouter(Point center)
        {
            Center = center;
            rectangles = new List<Rectangle>();
            Spiral = new Spiral(0.0005, 0);
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Height <= 0 || rectangleSize.Width <= 0)
                throw new ArgumentException("Size should be positive");
            var nextRectangle = GenerateNextRectangle(rectangleSize);
            rectangles.Add(nextRectangle);
            return nextRectangle;
        }


        private Rectangle GenerateNextRectangle(Size rectangleSize)
        {
            if (rectangles.Any())
            {
                while (true)
                {
                    var rectangleCenter = Spiral.GetNextPoint(Center);
                    var nexRectangle = new Rectangle(rectangleCenter, rectangleSize)
                        .ShiftRectangleToTopLeftCorner();
                    if (!rectangles.Any(nexRectangle.IntersectsWith))
                        return nexRectangle;
                }
            }
            return new Rectangle(Center, rectangleSize).ShiftRectangleToTopLeftCorner();
        }

        private int GetRadius()
        {
            return rectangles.Any() ?
                rectangles
                .Select(rect => new Point(MathHelper.MaxAbs(rect.Left, rect.Right), MathHelper.MaxAbs(rect.Top, rect.Bottom)))
                .Select(point => point.GetDistanceTo(Center)).Max() : 0;
        }
    }
}
