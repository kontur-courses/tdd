using System;
using System.Linq;
using System.Drawing;
using System.Collections.Generic;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        private const double AngleStep = Math.PI / 4;

        private Spiral _spiral { get; }
        private readonly Point _center;

        public List<Rectangle> Rectangles { get; } = new List<Rectangle>();

        public CircularCloudLayouter(Point center)
        {
            _center = center;
            _spiral = new Spiral(AngleStep, _center);
        }

        public CircularCloudLayouter() => _spiral = new Spiral(AngleStep, _center);

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Width < 0 || rectangleSize.Height < 0)
                throw new ArgumentException("Size values must be non-negative");

            var nextLocation = MoveFromItselfCenter(_center, rectangleSize);
            if (Rectangles.Count == 0)
                _spiral.FirstLayerRadius = rectangleSize.Width / 2 + 1;
            else
                nextLocation = CorrectLocation(_spiral.GetNextLocation(), rectangleSize);

            var newRectangle = new Rectangle(nextLocation, rectangleSize);
            _spiral.CurrentDensity = Math.Round(rectangleSize.GetDiagonalLength() / 2);
            Rectangles.Add(newRectangle);
            return newRectangle;
        }

        private static Point MoveFromItselfCenter(Point location, Size rectangleSize)
        {
            var xPosition = location.X - rectangleSize.Width / 2;
            var yPosition = location.Y + rectangleSize.Height / 2;
            return new Point(xPosition, yPosition);
        }

        private Point CorrectLocation(Point location, Size size)
        {
            var rectangle = new Rectangle(location, size);
            if (!CheckIntersections(rectangle, out var intersectedRectangles))
                return location;
            foreach (var intersectedRectangle in intersectedRectangles)
            {
                var correctionOffset = GetCorrectionOffset(intersectedRectangle, rectangle);
                location.Offset(correctionOffset);
            }

            return location;
        }

        private bool CheckIntersections(Rectangle rectangle, 
                                        out List<Rectangle> intersectedRectangles)
        {
            intersectedRectangles = Rectangles.Where(rectangle.IntersectsWith).ToList();
            return intersectedRectangles.Any();
        }

        private Point GetCorrectionOffset(Rectangle oldRectangle, Rectangle newRectangle)
        {
            var offsetDirection = GetOffsetDirection(oldRectangle, newRectangle, _center);
            int xOffset, yOffset;
            
            if (offsetDirection.X >= 0) xOffset = oldRectangle.Right - newRectangle.Left;
            else xOffset = newRectangle.Right - oldRectangle.Left;
            if (offsetDirection.Y >= 0) yOffset = oldRectangle.Top - newRectangle.Bottom;
            else yOffset = newRectangle.Top - oldRectangle.Bottom;
            
            xOffset = Math.Abs(xOffset) * offsetDirection.X;
            yOffset = Math.Abs(yOffset) * offsetDirection.Y;
            return new Point(xOffset, yOffset);
        }

        private static Point GetOffsetDirection(Rectangle oldRectangle, Rectangle newRectangle, Point center)
        {
            var oldRectCenter = oldRectangle.GetCenter();
            var newRectCenter = newRectangle.GetCenter();
            var xDirection = GetDirectionFromCenter(newRectCenter.X, oldRectCenter.X, center.X);
            var yDirection = GetDirectionFromCenter(newRectCenter.Y, oldRectCenter.Y, center.Y);
            return new Point(xDirection, yDirection);
        }

        internal static int GetDirectionFromCenter(int coordinate1, int coordinate2, int centerCoordinate)
        {
            var direction = Math.Sign(coordinate1 - coordinate2);
            return direction == 0 ? Math.Sign(centerCoordinate - coordinate1) : direction;
        }
    }
}