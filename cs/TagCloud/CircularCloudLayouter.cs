using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagCloud
{
    public class CircularCloudLayouter : ICloudLayouter
    {
        public Point CloudCenter { get; }

        private readonly ArchimedeanSpiral spiral;

        private readonly List<Rectangle> rectangles = new List<Rectangle>();

        public CircularCloudLayouter(Point cloudCenter)
        {
            CloudCenter = cloudCenter;

            spiral = new ArchimedeanSpiral(CloudCenter);
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (!IsValidRectangleSize(rectangleSize))
                throw new ArgumentException("width and height of rectangle must be more than zero");

            Rectangle rectangle;

            do
            {
                var pointToPutRectangle = spiral.GetNextPoint();

                rectangle = new Rectangle(pointToPutRectangle, rectangleSize);

            } while (IsIntersectWithAnyExistingRectangle(rectangle));

            var shiftedRectangle = ShiftRectangleToCenterPoint(rectangle);

            rectangles.Add(shiftedRectangle);

            return shiftedRectangle;
        }

        private static bool IsValidRectangleSize(Size rectangleSize)
        {
            return rectangleSize.Width > 0 && rectangleSize.Height > 0;
        }

        private bool IsIntersectWithAnyExistingRectangle(Rectangle rectangle)
        {
            return rectangles.Any(r => r.IntersectsWith(rectangle));
        }

        private Rectangle ShiftRectangleToCenterPoint(Rectangle rectangle)
        {
            var directionsToShift = GetDirectionsToShift(rectangle);

            return directionsToShift.Aggregate(rectangle, ShiftRectangleAlongDirection);
        }

        private IEnumerable<Vector> GetDirectionsToShift(Rectangle rectangle)
        {
            var deltaX = CloudCenter.X - rectangle.GetCenter().X > 0 ? 1 : -1;

            var deltaY = CloudCenter.Y - rectangle.GetCenter().Y > 0 ? 1 : -1;

            return new[] { new Vector(deltaX, 0), new Vector(0, deltaY) };
        }

        private Rectangle ShiftRectangleAlongDirection(Rectangle rectangle, Vector direction)
        {
            while (TryShiftRectangleAlongDirection(rectangle, direction, out var shiftedRectangle))
            {
                rectangle = shiftedRectangle;
            }

            return rectangle;
        }

        private bool TryShiftRectangleAlongDirection(Rectangle rectangle, Vector direction, out Rectangle shiftedRectangle)
        {
            if (IsRectangleAlignedAlongDirection(rectangle, direction))
            {
                shiftedRectangle = rectangle;

                return false;
            }

            var shiftedLocation = rectangle.Location.MoveOn(direction.X, direction.Y);

            shiftedRectangle = new Rectangle(shiftedLocation, rectangle.Size);

            return !IsIntersectWithAnyExistingRectangle(shiftedRectangle);
        }

        private bool IsRectangleAlignedAlongDirection(Rectangle rectangle, Vector direction)
        {
            var vectorBetweenCenters = new Vector(CloudCenter, rectangle.GetCenter());

            return direction.IsPerpendicularTo(vectorBetweenCenters);
        }
    }
}
