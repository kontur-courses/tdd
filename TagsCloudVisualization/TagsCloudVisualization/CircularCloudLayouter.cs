using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        private Point _center;
        private readonly List<Rectangle> _placedRectangles = new List<Rectangle>();

        public CircularCloudLayouter(Point center)
        {
            _center = center;
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
                throw new ArgumentException("rectangleSize is not correct rectangle size");

            var (x, y) = GetNextPosition(rectangleSize);
            var newRectangle = new Rectangle(x, y, rectangleSize.Width, rectangleSize.Height);
            while (_placedRectangles.Any(pr => pr.IntersectsWith(newRectangle)))
                (newRectangle.X, newRectangle.Y) = GetNextPosition(newRectangle.Size);

            _placedRectangles.Add(newRectangle);
            return newRectangle;
        }

        /**
         * Advance through the Archimedean spiral
         * Position is moved to correspond the upper left corner of rectangle with the given size
         */
        private double _phi = 0;
        private const double SpiralFactor = 0.5;
        private const double Step = 1 / 50d;

        private (int x, int y) GetNextPosition(Size size)
        {
            var x = (int) (_center.X + SpiralFactor * _phi * Math.Cos(_phi) - size.Width / 2d);
            var y = (int) (_center.Y + SpiralFactor * _phi * Math.Sin(_phi) - size.Height / 2d);
            _phi += Step;
            return (x, y);
        }
    }
}