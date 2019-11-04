using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagCloud
{
    public class CircularCloudLayouter
    {
        private ISpiralLayouter internalLayouter;
        private readonly Point center;
        private readonly List<Rectangle> rectangleMap;

        public CircularCloudLayouter(Point center)
        {
            this.center = center;
            internalLayouter = new СoncentricCircles();
            rectangleMap = new List<Rectangle>();
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Width == 0 || rectangleSize.Height == 0)
                throw new ArgumentException("rectangleSize is degenerate");
            // прямоугольники не сдвинуты, потому что ISpiralsLayouter рассчитывает,
            // что центр находится в начале координат
            var rect = internalLayouter.PutNextRectangle(rectangleSize, IsIntersects);
            rectangleMap.Add(rect);
            // lower ro
            return rect.CenterShift(center);
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