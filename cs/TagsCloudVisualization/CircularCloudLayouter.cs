using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter : ICloudLayouter
    {
        public readonly Point Center;
        private readonly List<Rectangle> currentRectangles;
        private int currentRadius;
        private double currentAngle;
        private const int shift = 1;
        public CircularCloudLayouter(Point center)
        {
            this.Center = center;
            currentRectangles = new List<Rectangle>();
            currentRadius = 0;
            currentAngle = 0;
        }
        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (currentRectangles.Count == 0)
                return PutFirstRectangle(rectangleSize);
            Rectangle? newRectangle = null;
            while(true)
            {
                var point = GetNextPoint();
                var rectangles = GetRectangles(point, rectangleSize);
                foreach (var rectangle in rectangles)
                {
                    if (rectangle.IntersectsWithRectangles(currentRectangles))
                        continue;
                    var movedRectangle = MoveToCenter(rectangle);
                    if (newRectangle == null)
                        newRectangle = movedRectangle;
                    else if (movedRectangle.GetDistanceToPoint(Center) < newRectangle.Value.GetDistanceToPoint(Center))
                        newRectangle = movedRectangle;
                }
                if (newRectangle != null)
                {
                    currentRectangles.Add(newRectangle.Value);
                    return newRectangle.Value;
                }
            }
        }

        private Rectangle PutFirstRectangle(Size rectangleSize)
        {
            var firstRectangle = new Rectangle(
                    new Point(
                        Center.X - rectangleSize.Width / 2,
                        Center.Y - rectangleSize.Height / 2),
                    rectangleSize);
            currentRectangles.Add(firstRectangle);
            return firstRectangle;
        }

        private Point GetNextPoint()
        {
            if (currentRadius == 0)
            {
                currentRadius++;
                return Center;
            }
            var point = new Point(
                (int)(Center.X + currentRadius * Math.Cos(currentAngle)),
                (int)(Center.Y + currentRadius * Math.Sin(currentAngle)));
            currentAngle += Math.PI / (6 + currentRadius);
            if (Math.Abs(currentAngle - 2 * Math.PI) < 0.0000001)
            {
                currentAngle = 0;
                currentRadius++;
            }
            return point;
        }

        private Rectangle[] GetRectangles(Point point, Size size)
        {
            return new Rectangle[]
            {
                new Rectangle(point.X - size.Width, point.Y, size.Width, size.Height),
                new Rectangle(point, size),
                new Rectangle(point.X, point.Y - size.Height, size.Width, size.Height),
                new Rectangle(point.X - size.Width, point.Y - size.Height, size.Width, size.Height)
            };
        }

        private Rectangle MoveToCenter(Rectangle rectangle)
        {
            var resultRectangle = new Rectangle(rectangle.Location, rectangle.Size);
            while (true)
            {
                var wasMoved = false;
                foreach (var direction in Utils.GetAllDirections())
                {
                    var tempRectangle = resultRectangle.GetMovedCopy(direction, shift);
                    if (!tempRectangle.IntersectsWithRectangles(currentRectangles)
                        && tempRectangle.GetDistanceToPoint(Center) < resultRectangle.GetDistanceToPoint(Center))
                    {
                        resultRectangle.X = tempRectangle.X;
                        resultRectangle.Y = tempRectangle.Y;
                        wasMoved = true;
                        break;
                    }
                }
                if (!wasMoved)
                    return resultRectangle;
            }
        }
    }
}
