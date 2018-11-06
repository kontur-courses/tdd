using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
    class CircularCloudLayouter
    {
        private const double SpiralAngleInterval = 1;
        private const double MaxSpiralAngle = double.MaxValue;
        private const double SpiralTurnsInterval = 1;

        private Point center;
        private List<Rectangle> rectanglesList;

        public CircularCloudLayouter(Point center)
        {
            this.center = center;
            rectanglesList = new List<Rectangle>();
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            var rectangle = PutOnSpiral(rectangleSize);
            if (rectangle == null)
                return null;
            rectangle = MakeCloserToCenter(rectangle);
            rectanglesList.Add(rectangle);
            return rectangle;
        }

        private Point FindDirectionToCenter(Quarter[] quarters)
        {
            var x = 0;
            var y = 0;

            if (quarters.Contains(Quarter.TopRight))
            {
                x -= 1;
                y -= 1;
            }

            if (quarters.Contains(Quarter.TopLeft))
            {
                x += 1;
                y -= 1;
            }

            if (quarters.Contains(Quarter.BottomLeft))
            {
                x += 1;
                y += 1;
            }

            if (quarters.Contains(Quarter.BottomRight))
            {
                x -= 1;
                y += 1;
            }

            return new Point(x, y).Normalized();
        }

        private Rectangle MakeCloserToCenter(Rectangle rectangle)
        {
            var quarters = rectangle.GetQuarters();
            var movingPoint = FindDirectionToCenter(quarters);
            if (movingPoint.Equals(new Point(0, 0)))
                return rectangle;
            var previousPosition = new Point(0, 0);
            while (!rectangle.IsIntersectsWithAnyRect(rectanglesList))
            {
                previousPosition = rectangle.Center;
                rectangle.Center += movingPoint;
            }

            rectangle.Center = previousPosition;
            return rectangle;
        }

        private Rectangle PutOnSpiral(Size rectangleSize)
        {
            var newRectangle = new Rectangle(center, center, rectangleSize);
            for (var angle = 0d; angle < MaxSpiralAngle; angle += SpiralAngleInterval)
            {
                var rectCenter = ArithmeticSpiral(angle, SpiralTurnsInterval);
                newRectangle.Center = rectCenter;
                if (!newRectangle.IsIntersectsWithAnyRect(rectanglesList))
                    return newRectangle;
            }

            return null;
        }

        private Point ArithmeticSpiral(double angle, double turnsInterval)
        {
            var x = (center.X + turnsInterval * angle) * Math.Cos(angle);
            var y = (center.Y + turnsInterval * angle) * Math.Sin(angle);

            return new Point(x, y);
        }
    }
}
