using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter : ICloudLayouter
    {
        private readonly Point center;
        private readonly List<Rectangle> currentRectangles;
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
            this.center = center;
            currentRectangles = new List<Rectangle>();
            currentRadius = 0;
            currentAngle = 0;
        }
        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            var point = GetNextPoint();
            if (point == center)
            {
                currentRectangles.Add(new Rectangle(
                    new Point(
                        center.X - rectangleSize.Width / 2,
                        center.Y - rectangleSize.Height / 2),
                    rectangleSize));
                return currentRectangles[0];
            }
            var newRectangle = new Rectangle(point, rectangleSize);
            while(IntersectWithPreviousRectangles(newRectangle))
            {
                point = GetNextPoint();
                newRectangle = new Rectangle(point, rectangleSize);
            }
            currentRectangles.Add(MoveToCenter(newRectangle));
            return newRectangle;
        }

        private Point GetNextPoint()
        {
            if (currentRadius == 0)
            {
                currentRadius++;
                return center;
            }
            var point = new Point(
                (int)(center.X + currentRadius * Math.Cos(currentAngle)),
                (int)(center.Y + currentRadius * Math.Sin(currentAngle)));
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
            foreach (var rectangle in currentRectangles)
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
            return Math.Sqrt((center.X - rectangle.X) ^ 2 + (center.Y - rectCenter.Y) ^ 2);
        }

        private Rectangle MoveToCenter(Rectangle rectangle)
        {
            var resultRectangle = new Rectangle(rectangle.Location, rectangle.Size);
            Rectangle tempRectangle = new Rectangle(rectangle.Location, rectangle.Size); ;
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
