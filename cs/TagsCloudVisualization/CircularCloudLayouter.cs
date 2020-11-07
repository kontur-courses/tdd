using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace TagsCloudVisualization
{
    class CircularCloudLayouter
    {
        public Point Center { get; }
        public List<Rectangle> Rectangles { get; }
        private int cloudRadius;

        public CircularCloudLayouter(Point center)
        {
            if (center.X < 0 || center.Y < 0)
                throw new ArgumentException($"Invalid coordinates of center = ({center.X}, {center.Y})");
            Center = center;
            Rectangles = new List<Rectangle>();
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
                throw new ArgumentException($"Invalid rectangleSize = ({rectangleSize.Width}, {rectangleSize.Height})");
            var spiral = new Spiral(cloudRadius, Center);
            while (true)
            {
                var currentCoordinate = spiral.GetCurrentPosition(rectangleSize);
                var currentRectangle = new Rectangle(currentCoordinate, rectangleSize);

                if (!CheckIntersections(currentRectangle))
                {
                    ShiftRectangleToCenter(ref currentRectangle);
                    cloudRadius = Math.Max(currentRectangle.Right - Center.X, cloudRadius);
                    Rectangles.Add(currentRectangle);
                    return currentRectangle;
                }
                spiral.TakeStep();
            }
        }

        private void ShiftRectangleToCenter(ref Rectangle rectangle, int step = 1)
        {
            var previousPosition = rectangle.Location;
            while (true)
            {
                ShiftHorizontal(ref rectangle, previousPosition.X, step);
                ShiftVertical(ref rectangle, previousPosition.Y, step);
                if (previousPosition == rectangle.Location)
                    break;
                previousPosition = rectangle.Location;
            }
        }

        private void ShiftVertical(ref Rectangle rectangle, int availablePositionY, int step)
        {
            if (Math.Abs(availablePositionY - Center.Y + rectangle.Height / 2) > 1)
            {
                step = availablePositionY >= Center.Y - rectangle.Height / 2 ? -step : step;
                rectangle.Offset(0, step);
                if (CheckIntersections(rectangle))
                    rectangle.Offset(0, -step);
            }
        }

        private void ShiftHorizontal(ref Rectangle rectangle, int availablePositionX, int step)
        {
            if (Math.Abs(availablePositionX - Center.X + rectangle.Width / 2) > 1)
            {
                step = availablePositionX >= Center.X - rectangle.Width / 2 ? -step : step;
                rectangle.Offset(step, 0);
                if (CheckIntersections(rectangle))
                    rectangle.Offset(-step, 0);
            }
        }

        private bool CheckIntersections(Rectangle rectangle)
        { 
            foreach (var otherRectangle in Rectangles)
                if (rectangle.IntersectsWith(otherRectangle))
                    return true; 

            return false;
        }
    }
}
