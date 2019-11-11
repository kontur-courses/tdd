using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagCloud
{
    public class CircularCloudLayouter
    {
        public readonly ArchimedeanSpiral Spiral;
        public readonly HashSet<Rectangle> Rectangles;
        public readonly Size screenSize;

        public CircularCloudLayouter(Point center, Size screenSize)
        {
            this.screenSize = screenSize;
            CheckCorrectPointPosition(center);
            CheckCorrectSize(screenSize);
            Spiral = new ArchimedeanSpiral(center, 0.01, 0.1);
            Rectangles = new HashSet<Rectangle>();
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            CheckCorrectSize(rectangleSize);
            foreach (var point in Spiral.GetNewPointLazy())
            {
                var rect = new Rectangle(point, rectangleSize);
                if (!CheckCorrectRectanglePosition(rect)) return new Rectangle(point, Size.Empty);
                if (!RectangleDoesNotIntersect(rect)) continue;
                Rectangles.Add(rect);
                return rect;
            }

            throw new Exception("Rectangle should be added anyway");
        }

        private void CheckCorrectSize(Size rectangleSize)
        {
            if (rectangleSize.Width <= 0
                || rectangleSize.Width > screenSize.Width
                || rectangleSize.Height <= 0
                || rectangleSize.Height > screenSize.Height)
                throw new ArgumentException("Incorrect size of rectangle");
        }

        private bool CheckCorrectRectanglePosition(Rectangle rect)
        {
            return rect.Location.X + rect.Size.Width <= screenSize.Width / 2 &&
                   rect.Location.X >= -screenSize.Width / 2 &&
                   rect.Location.Y + rect.Size.Height <= screenSize.Height / 2 &&
                   rect.Location.Y >= -screenSize.Height / 2;
        }

        private void CheckCorrectPointPosition(Point point)
        {
            if (point.X > screenSize.Width / 2
                || point.X < -screenSize.Width / 2
                || point.Y > screenSize.Height / 2
                || point.Y < -screenSize.Height / 2)
                throw new ArgumentException();
        }

        private bool RectangleDoesNotIntersect(Rectangle rectToAdd)
        {
            return Rectangles.All(rectangle => Rectangle.Intersect(rectToAdd, rectangle).IsEmpty);
        }
    }
}