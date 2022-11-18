using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagsCloudVisualization.Interfaces;
using TagsCloudVisualization.Utils;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter : IRectangleLayouter
    {
        private readonly List<Rectangle> rectangles;

        public CircularCloudLayouter(Point center)
        {
            Center = center;
            rectangles = new List<Rectangle>();
        }

        public Point Center { get; }

        public IReadOnlyList<Rectangle> Rectangles => rectangles;

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Height <= 0)
                throw new ArgumentOutOfRangeException($"Height ({rectangleSize.Height}) must be positive");
            if (rectangleSize.Width <= 0)
                throw new ArgumentOutOfRangeException($"Width ({rectangleSize.Width}) must be positive");

            Rectangle newRectangle;

            if (rectangles.Count == 0)
                newRectangle = new Rectangle(Center - rectangleSize / 2, rectangleSize);
            else
                newRectangle = FindPlaceForRectangle(rectangleSize);

            rectangles.Add(newRectangle);
            return newRectangle;
        }

        private Rectangle FindPlaceForRectangle(Size rectangleSize)
        {
            var rectangleScale = Math.Max(rectangleSize.Height, rectangleSize.Width);

            // So that angles never recur and are distributed evenly:
            var startAngleOfSpiral = rectangles.Count;

            foreach (var rectangleCenter in
                SpiralPointScatterer.Scatter(Center, rectangleScale, startAngleOfSpiral))
            {
                var rectangle = RectangleExtensions.NewRectangle(rectangleCenter.ToPoint(), rectangleSize);
                if (IntersectsWithExistingRectangles(rectangle))
                    continue;

                return rectangle;
            }

            throw new InvalidOperationException(
                $"Could not find a place for a rectangle of width {rectangleSize.Width} and height {rectangleSize.Height}");
        }

        public double GetCoveringCircleRadius()
        {
            return Rectangles
                .SelectMany(r => r.GetVertices())
                .Select(p => Center.GetDistanceTo(p))
                .Max();
        }


        private bool IntersectsWithExistingRectangles(Rectangle newRectangle)
        {
            return Rectangles.Any(r => r.IntersectsWith(newRectangle));
        }
    }
}