using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter : ICloudLayouter
    {
        public readonly Point Center;
        public readonly List<Rectangle> CurrentRectangles;
        private int currentRadius;
        private double currentAngle;
        public readonly Tuple<int, int>[] Directions = new Tuple<int, int>[]
            {
                new Tuple<int, int>(0, -1),
                new Tuple<int, int>(0, 1),
                new Tuple<int, int>(-1, 0),
                new Tuple<int, int>(1, 0)
            };
        public readonly int Shift = 2;
        public CircularCloudLayouter(Point center)
        {
            this.Center = center;
            CurrentRectangles = new List<Rectangle>();
            currentRadius = 0;
            currentAngle = 0;
        }
        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (CurrentRectangles.Count == 0)
                return PutFirstRectangle(rectangleSize);
            Rectangle? newRectangle = null;
            while(true)
            {
                var point = GetNextPoint();
                var rectangles = GetRectangles(point, rectangleSize);
                foreach (var rectangle in rectangles)
                {
                    if (IntersectWithPreviousRectangles(rectangle))
                        continue;
                    var movedRectangle = MoveToCenter(rectangle);
                    if (newRectangle == null)
                        newRectangle = movedRectangle;
                    else if (GetDistanceToCenter(movedRectangle) < GetDistanceToCenter((Rectangle)newRectangle))
                        newRectangle = movedRectangle;
                }
                if (newRectangle != null)
                    break;
            }
            CurrentRectangles.Add((Rectangle)newRectangle);
            return (Rectangle)newRectangle;
        }

        private Rectangle PutFirstRectangle(Size rectangleSize)
        {
            var firstRectangle = new Rectangle(
                    new Point(
                        Center.X - rectangleSize.Width / 2,
                        Center.Y - rectangleSize.Height / 2),
                    rectangleSize);
            CurrentRectangles.Add(firstRectangle);
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

        public bool IntersectWithPreviousRectangles(Rectangle newRectangle)
        {
            foreach (var rectangle in CurrentRectangles)
            {
                if (newRectangle.IntersectsWith(rectangle))
                    return true;
            }
            return false;
        }

        public double GetDistanceToCenter(Rectangle rectangle)
        {
            var rectCenter = new Point(
                rectangle.X + rectangle.Width / 2,
                rectangle.Y + rectangle.Height / 2);
            return Math.Sqrt((Center.X - rectangle.X) ^ 2 + (Center.Y - rectCenter.Y) ^ 2);
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
            Rectangle tempRectangle = new Rectangle(rectangle.Location, rectangle.Size);
            while (true)
            {
                var flag = false;
                foreach (var direction in Directions)
                {
                    tempRectangle.X = resultRectangle.X + direction.Item1 * Shift;
                    tempRectangle.Y = resultRectangle.Y + direction.Item2 * Shift;
                    if (!IntersectWithPreviousRectangles(tempRectangle) 
                        && GetDistanceToCenter(tempRectangle) < GetDistanceToCenter(resultRectangle))
                    {
                        resultRectangle.X = tempRectangle.X;
                        resultRectangle.Y = tempRectangle.Y;
                        flag = true;
                        break;
                    }
                }
                if (!flag)
                    break;
            }
            return resultRectangle;
        }
    }
}
