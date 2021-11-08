using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        public readonly Size Size;

        private readonly List<Rectangle> rectangles;
        private readonly SpiralPointsGenerator pointsGenerator;

        public CircularCloudLayouter() : this(new Point(0, 0))
        {
        }

        public CircularCloudLayouter(Point center, double startRadius = 10, double startAngle = 0,
            double angleDelta = Math.PI / 180, double radiusDelta = 0.01)
        {
            Size = new Size(center.X * 2, center.Y * 2);
            rectangles = new List<Rectangle>();
            pointsGenerator = new SpiralPointsGenerator(center, startRadius, startAngle, angleDelta, radiusDelta);
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Width < 0 || rectangleSize.Height < 0)
            {
                throw new ArgumentException($"Width and height can't be negative");
            }

            rectangles.Add(new Rectangle(GetNextPointForRectangle(rectangleSize), rectangleSize));
            return rectangles.Last();
        }

        private Point GetNextPointForRectangle(Size rectangleSize)
        {
            return pointsGenerator.GetSpiralPoints()
                .FirstOrDefault(p => !IntersectsWithRectangles(new Rectangle(p, rectangleSize)));
        }

        private bool IntersectsWithRectangles(Rectangle rectangle)
        {
            return rectangles.Any(r => r.IntersectsWith(rectangle));
        }
    }
}