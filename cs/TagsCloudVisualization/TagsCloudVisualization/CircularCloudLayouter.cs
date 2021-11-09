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

        public CircularCloudLayouter() : this(new SpiralPointsGenerator())
        {
        }

        public CircularCloudLayouter(SpiralPointsGenerator pointsGenerator)
        {
            Size = pointsGenerator.Size;
            rectangles = new List<Rectangle>();
            this.pointsGenerator = pointsGenerator;
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
            return rectangles
                .Any(r => r.IntersectsWith(rectangle));
        }
    }
}