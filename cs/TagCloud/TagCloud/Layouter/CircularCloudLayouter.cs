using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagCloud.Layouter;

namespace TagCloud
{
    public class CircularCloudLayouter
    {
        private ISpiral spiral;
        private readonly Point center;
        private readonly List<Rectangle> rectangleMap;

        public CircularCloudLayouter(Point center)
        {
            this.center = center;
            spiral = new СoncentricCircles();
            rectangleMap = new List<Rectangle>();
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Width == 0 || rectangleSize.Height == 0)
                throw new ArgumentException("rectangleSize is degenerate");
            var centerShift = new Point(-rectangleSize.Width / 2, - rectangleSize.Height / 2);
            foreach (var spiralPoint in spiral.IterateBySpiralPoints())
            {
                var rect = new Rectangle(new Point(spiralPoint.X + centerShift.X, spiralPoint.Y + centerShift.Y), rectangleSize).Shift(center);
                if (IsIntersects(rect)) 
                    continue;
                rectangleMap.Add(rect);
                return rect;
            }
            throw new ArgumentException("Spiral doesn't return appropriate points");
        }

        public IEnumerable<Rectangle> GetAllRectangles()
        {
            return rectangleMap.Select(rect => rect.Shift(center));
        }

        private bool IsIntersects(Rectangle target)
        {
            return rectangleMap.Any(rectangle => rectangle.IntersectsWith(target));
        }
    }
}