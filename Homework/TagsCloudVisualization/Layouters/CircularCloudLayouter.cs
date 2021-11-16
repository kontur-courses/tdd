using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagsCloudVisualization.Extensions;

namespace TagsCloudVisualization.Layouters
{
    public class CircularCloudLayouter
    {
        public readonly HashSet<RectangleF> rectangles;
        private readonly Point center;
        private readonly IPointLayouter pointLayouter;

        public CircularCloudLayouter(Point center)
        {
            pointLayouter = new Spiral(center);
            rectangles = new HashSet<RectangleF>();
        }

        public RectangleF PutNextRectangle(SizeF rectangleSize)
        {
            if (rectangleSize.Height <= 0 || rectangleSize.Width <= 0)
            {
                throw new ArgumentException("Rectangle size should be positive floating point numbers");
            }

            var rect = GetNextRectanglePosition(rectangleSize);

            rectangles.Add(rect);

            return rect;
        }

        // ReSharper disable once UnusedMember.Global
        public IEnumerable<RectangleF> PutNextRectangles(IEnumerable<SizeF> rectanglesSizes)
            => rectanglesSizes.Select(PutNextRectangle);

        private RectangleF GetNextRectanglePosition(SizeF rectangleSize)
        {
            var rect = new RectangleF();
            do
            {
                rect = new RectangleF(
                    pointLayouter.CurrentPoint.X - rectangleSize.Width / 2,
                    pointLayouter.CurrentPoint.Y - rectangleSize.Height / 2,
                    rectangleSize.Height,
                    rectangleSize.Width);
                pointLayouter.GetNextPoint();
            } while (rect.IntersectsWithAnyOf(rectangles));

            return rect;
        }
    }
}