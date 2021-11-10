using System;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class SpiralPointsCreator
    {
        private const double AngleDelta = Math.PI / 12;

        private double currentAngle;
        private Point? lastPoint;

        public SpiralPointsCreator(Point center)
        {
            Center = center;
        }

        public Point Center { get; }

        public Point GetNextPoint()
        {
            Point currentPoint;
            do
            {
                var radiusVector = currentAngle;
                var newX = Center.X + radiusVector * Math.Cos(currentAngle);
                var newY = Center.Y + radiusVector * Math.Sin(currentAngle);
                var roundedX = (int) Math.Round(newX);
                var roundedY = (int) Math.Round(newY);
                currentPoint = new Point(roundedX, roundedY);
                currentAngle += AngleDelta;
            } while (currentPoint == lastPoint);

            lastPoint = currentPoint;
            return currentPoint;
        }
    }
}