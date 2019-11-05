using System;
using System.Linq;
using System.Drawing;
using System.Collections.Generic;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        private const double AngleStep = Math.PI / 4;
        private const int CorrectionMoveDistance = 1;

        internal Spiral _spiral;
        private readonly Point _center;

        public readonly List<Rectangle> Rectangles = new List<Rectangle>();
        
        public CircularCloudLayouter(Point center) => _center = center;

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Width < 0 || rectangleSize.Height < 0)
                throw new ArgumentException("Size values must be non-negative");

            var nextLocation = MoveFromItselfCenter(_center, rectangleSize);
            if (Rectangles.Count == 0)
                InitializeSpiral(rectangleSize);
            else
                nextLocation = CorrectLocation(_spiral.GetNextLocation(), rectangleSize);

            var newRectangle = new Rectangle(nextLocation, rectangleSize);
            _spiral.UpdateDensity(rectangleSize);
            Rectangles.Add(newRectangle);
            return newRectangle;
        }

        internal void InitializeSpiral(Size rectangleSize)
        {
            var firstLayerRadius = rectangleSize.Width / 2 + 1;
            var density = Spiral.CalculateDensity(rectangleSize);
            _spiral = new Spiral(AngleStep, firstLayerRadius, density, _center);
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
            while (CheckIntersections(rectangle))
                rectangle = MoveFromCenter(rectangle, CorrectionMoveDistance, _center);
            return rectangle.Location;
        }

        private bool CheckIntersections(Rectangle rectangle) => 
            Rectangles.Where(rectangle.IntersectsWith).ToList().Any();

        internal static Rectangle MoveFromCenter(Rectangle rectangle, int moveDistance, Point center)
        {
            var rectCenterX = rectangle.X + rectangle.Width / 2;
            var rectCenterY = rectangle.Y - rectangle.Height / 2;
            var xOffset = moveDistance * GetMoveDirection(rectCenterX, center.X);
            var yOffset = moveDistance * GetMoveDirection(rectCenterY, center.Y);
            rectangle.Offset(xOffset, yOffset);
            return rectangle;
        }

        private static int GetMoveDirection(int coordinate, int centerCoordinate) => 
            coordinate > centerCoordinate ? 1 : -1;
    }
}