using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagCloud
{
    public class CircularCloudLayouter
    {
        private ISpiral _internal;
        private readonly Point center;
        private readonly List<Rectangle> rectangleMap;

        public CircularCloudLayouter(Point center)
        {
            this.center = center;
            _internal = new СoncentricCircles();
            rectangleMap = new List<Rectangle>();
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Width == 0 || rectangleSize.Height == 0)
                throw new ArgumentException("rectangleSize is degenerate");
            var centerShift = new Point(-rectangleSize.Width / 2, - rectangleSize.Height / 2);
            foreach (var spiralPoint in _internal.IterateBySpiralPoints())
            {
                var rect = new Rectangle(new Point(spiralPoint.X + centerShift.X, spiralPoint.Y + centerShift.Y), rectangleSize).CenterShift(center);
                if (!IsIntersects(rect))
                {
                    rectangleMap.Add(rect);
                    return rect;
                }
            }
            throw new ArgumentException("Spiral doesn't return appropriate points");
        }

        public IEnumerable<Rectangle> GetAllRectangles()
        {
            return rectangleMap.Select(rect => rect.CenterShift(center));
        }

        private bool IsIntersects(Rectangle target)
        {
            return rectangleMap.Any(rectangle => rectangle.IntersectsWith(target));
        }
    }

    public static class RectangleExtension
    {
        public static Rectangle CenterShift(this Rectangle rect, Point center)
        {
            return new Rectangle(new Point(rect.X + center.X, rect.Y + center.Y), rect.Size);     
        }
    }
}