using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagCloud.Layouting
{
    public class CircularCloudLayouter : ICloudLayouter
    {
        private readonly List<Rectangle> rectangles;
        private DirectingArrow arrow;

        public CircularCloudLayouter(Point center)
        {
            Center = center;
            rectangles = new List<Rectangle>();
        }

        public Point Center { get; }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            ThrowIfIncorrectSize(rectangleSize);
            var rect = new Rectangle(Point.Empty, rectangleSize);

            if (rectangles.Count == 0)
            {
                rect.Location = new Point(Center.X - rect.Width / 2,
                    Center.Y - rect.Height / 2);

                arrow = InitArrow(rect);
            }

            else
            {
                rect.Location = new Point(arrow.EndPoint.X - rect.Width / 2,
                    arrow.EndPoint.Y - rect.Height / 2);
                arrow.Rotate();
            }

            rectangles.Add(rect);
            return rect;
        }

        public int GetCloudBoundaryRadius()
        {
            var boundaryBox = GetRectanglesBoundaryBox();
            var biggestSide = Math.Max(boundaryBox.Width, boundaryBox.Height);
            return (int)Math.Ceiling(biggestSide * (Math.Sqrt(2) / 2));
        }

        public Size GetRectanglesBoundaryBox()
        {
            var width
                = rectangles.Max(rect => rect.Right) - rectangles.Min(rect => rect.X);

            var height
                = rectangles.Max(rect => rect.Bottom) - rectangles.Min(rect => rect.Y);

            return new Size(width, height);
        }

        public List<Rectangle> GetRectanglesCopy()
        {
            return new List<Rectangle>(rectangles);
        }

        private void ThrowIfIncorrectSize(Size rectangleSize)
        {
            if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
                throw new ArgumentException("Width and height of rectangle must be a positive numbers");
        }

        private DirectingArrow InitArrow(Rectangle rectangle)
        {
            var startDirectionPoint = new Point(rectangle.X + rectangle.Width / 2,
                rectangle.Y - rectangle.Height);

            return new DirectingArrow(startDirectionPoint);
        }

        private class DirectingArrow
        {
            private const int DefaultAngleDegree = 0;
            private const int DefaultAngleDegreeIncrease = 45;

            private int currentCoilNumber = 1;

            public DirectingArrow(Point firstDirectPoint)
            {
                EndPoint = firstDirectPoint;
                AngleDegree = DefaultAngleDegree;
            }

            public Point EndPoint { get; private set; }
            public int AngleDegree { get; private set; }

            public void Rotate()
            {
                var rotationAngle = DefaultAngleDegreeIncrease / currentCoilNumber;
                AngleDegree += rotationAngle;

                if (AngleDegree > 360)
                {
                    currentCoilNumber++;
                    AngleDegree %= 360;
                    EndPoint = new Point(EndPoint.X * 4, EndPoint.Y * 4);
                }

                var newX = (int)(EndPoint.X * Math.Cos(rotationAngle)
                                 + EndPoint.Y * Math.Sin(rotationAngle));

                var newY = (int)(EndPoint.X * Math.Cos(rotationAngle)
                                 - EndPoint.Y * Math.Cos(rotationAngle));

                EndPoint = new Point(newX, newY);
            }
        }
    }
}