using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagCloud
{
    public class CircularCloudLayouter
    {
        private readonly Point center;
        public readonly ArchimedeanSpiral Spiral;
        public readonly HashSet<Rectangle> Rectangles;

        public CircularCloudLayouter(Point center)
        {
            this.center = center;
            Spiral = new ArchimedeanSpiral(center);
            Rectangles = new HashSet<Rectangle>();
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            CheckCorrectSize(rectangleSize);
            foreach (var point in Spiral.GetNewPointLazy())
            {
                var rect = new Rectangle(point, rectangleSize);
                if (!RectangleDoesNotIntersect(rect)) continue;
                Rectangles.Add(rect);
                return rect;
            }

            throw new Exception("Rectangle should be added anyway");
        }

        private static void CheckCorrectSize(Size rectangleSize)
        {
            if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
                throw new ArgumentException();
        }

        private bool RectangleDoesNotIntersect(Rectangle rectToAdd)
        {
            return Rectangles.All(rectangle => Rectangle.Intersect(rectToAdd, rectangle).IsEmpty);
        }
    }
}