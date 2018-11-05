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

        private Spiral Spiral { get; }

        public int Radius => GetRadius();

        public CircularCloudLayouter(Point center)
        {
            Center = center;
            Rectangles = new List<Rectangle>();
            Spiral = new Spiral(0.0005, 0);
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
                    var nexRectangle = new Rectangle(rectangleCenter, rectangleSize)
                        .ShiftRectangleToTopLeftCorner();
                    if (!Rectangles.Any(nexRectangle.IntersectsWith))
                        return nexRectangle;
                }
            }
            return new Rectangle(Center, rectangleSize).ShiftRectangleToTopLeftCorner();
        }

        private int GetRadius()
        {
            return Rectangles
                .Select(rect => new Point(MaxAbs(rect.Left, rect.Right), MaxAbs(rect.Top, rect.Bottom)))
                .Select(point => (int)Math.Sqrt(Math.Pow(point.X - Center.X, 2) + Math.Pow(point.Y - Center.Y, 2))).Max();
        }

        private int MaxAbs(int val1, int val2)
        {
            return Math.Abs(val1) == Math.Max(Math.Abs(val1), Math.Abs(val2)) ? val1 : val2;
        }
    }
}
