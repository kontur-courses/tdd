using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        private const double DefaultAngleDelta = Math.PI / 6;
        private readonly Spiral spiral;
        private readonly List<Rectangle> rectangles = new List<Rectangle>();

        public CircularCloudLayouter(Point center)
        {
            spiral = new Spiral(DefaultAngleDelta, center);
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
                throw new ArgumentException($"{nameof(rectangleSize)} was incorrect");

            if (spiral.Step < Math.Max(rectangleSize.Height, rectangleSize.Width))
            {
                spiral.Step = Math.Max(rectangleSize.Height, rectangleSize.Width);
            }

            var nextRectCenter = spiral.GetNextPoint();
            var rectangle = nextRectCenter.GetRectangleFromCenterPoint(rectangleSize);

            while (rectangles.Any(r => r.IntersectsWith(rectangle)))
            {
                nextRectCenter = spiral.GetNextPoint();
                rectangle = nextRectCenter.GetRectangleFromCenterPoint(rectangleSize);
            }

            rectangles.Add(rectangle);
            return rectangle;
        }
    }
}