using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagsCloudVisualization.Extensions;

namespace TagsCloudVisualization.Layouters
{
    public class CircularCloudLayouter
    {
        private readonly Point center;
        private readonly HashSet<RectangleF> rectangles;
        private readonly Spiral spiral;
        
        public CircularCloudLayouter(Point center)
        {
            spiral = new Spiral(center);
            rectangles = new HashSet<RectangleF>();
        }

        public RectangleF PutNextRectangle(SizeF rectangleSize)
        {
            if (rectangleSize.Height <= 0 || rectangleSize.Width <= 0)
            {
                throw new ArgumentException("Rectangle size should be positive floating point numbers");
            }
            
            var rect = FindRectanglePosition(rectangleSize);
            
            rectangles.Add(rect);
            
            return rect;
        }

        public IEnumerable<RectangleF> PutNextRectangles(IEnumerable<SizeF> rectanglesSizes)
            => rectanglesSizes.Select(PutNextRectangle);

        private RectangleF FindRectanglePosition(SizeF rectangleSize)
        {
            var rect = new RectangleF(
                spiral.CurrentPoint.X - rectangleSize.Width / 2,
                spiral.CurrentPoint.Y - rectangleSize.Height / 2,
                rectangleSize.Height,
                rectangleSize.Width);
            
            while (rect.IntersectsWithAny(rectangles))
            {
                spiral.IncreaseSize();
                rect = new RectangleF(
                    spiral.CurrentPoint.X - rectangleSize.Width / 2,
                    spiral.CurrentPoint.Y - rectangleSize.Height / 2,
                    rectangleSize.Height,
                    rectangleSize.Width);
            }

            return rect;
        }
    }
}