using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;

namespace TagCloud.Layouting
{
    public class CircularCloudLayouter : ICloudLayouter
    {
        private class DirectingArrow
        {
            private const int DefaultAngleDegree = 90;
            private const int DefaultAngleDegreeIncrease = 45;

            private int currentCoilNumber = 1;

            public Point EndPoint { get; private set; }
            public int AngleDegree { get; private set; }

            public DirectingArrow(Point firstDirectPoint)
            {
                EndPoint = firstDirectPoint;
                AngleDegree = DefaultAngleDegree;
            }

            public void Rotate()
            {
                var rotationAngle = DefaultAngleDegreeIncrease / currentCoilNumber;
                AngleDegree += rotationAngle;

                if (AngleDegree > 360)
                {
                    currentCoilNumber++;
                    AngleDegree %= 360;
                    EndPoint = new Point(EndPoint.X * 2, EndPoint.Y * 2);
                }

                var newX = (int)(EndPoint.X * Math.Cos(rotationAngle) 
                                 - EndPoint.Y * Math.Sin(rotationAngle));

                var newY = (int)(EndPoint.X * Math.Cos(rotationAngle)
                                 + EndPoint.Y * Math.Cos(rotationAngle));

                EndPoint = new Point(newX, newY);
            }
        }

        private readonly Point center;
        private readonly List<Rectangle> rectangles;
        private DirectingArrow arrow;

        public CircularCloudLayouter(Point center)
        {
            this.center = center;
            rectangles = new List<Rectangle>();
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            ThrowIfIncorrectSize(rectangleSize);
            var rect = new Rectangle(Point.Empty, rectangleSize);

            if (rectangles.Count == 0)
            {
                rect.Location = new Point(center.X - rect.Width / 2,
                    center.Y - rect.Height / 2);

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

        public Point GetCloudCenter()
        {
            return center;
        }

        public int GetCloudRadius()
        {
            var boundaryBox = GetRectanglesBoundaryBox();
            var biggestSide = Math.Max(boundaryBox.Width, boundaryBox.Height);
            return (int)(biggestSide * (Math.Sqrt(2) / 2));
        }

        public Size GetRectanglesBoundaryBox()
        {
            var width
                = rectangles.Max(rect => rect.Right) - rectangles.Min(rect => rect.X);

            var height
                = rectangles.Max(rect => rect.Bottom) - rectangles.Min(rect => rect.Y);

            return new Size(width, height);
        }

        public List<Rectangle> GetRectangles()
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
                rectangle.Top - rectangle.Height);

            return new DirectingArrow(startDirectionPoint);
        }

    }
}