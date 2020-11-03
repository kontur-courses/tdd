using System;
using System.Drawing;

namespace TagsCloudVisualization.Infrastructure.Layout
{
    public class SpiralPlacing : ILayoutStrategy
    {
        private readonly Point center;
        private readonly int angleIncrement;
        public SpiralPlacing(Point center, int angleIncrement)
        {
            this.center = center;
            this.angleIncrement = angleIncrement;
        }

        public Point GetPoint(Func<Point, bool> isValidPoint)
        {
            var angle = 0;
            
            Point obtainedPoint;
            while (true)
            {
                var possiblePoint = center + new Size((int) (Math.Sin(angle) * angle), (int) (Math.Cos(angle) * angle));
                obtainedPoint = possiblePoint;
                if (isValidPoint(possiblePoint))
                    break;
                angle += angleIncrement;
            }
            return OptimizePoint(obtainedPoint, isValidPoint);;
        }

        private Point OptimizePoint(Point obtainedPoint, Func<Point, bool> isValidPoint)
        {
            while (true)
            {
                var optimizationOffset = new Size(
                    Math.Sign(center.X - obtainedPoint.X),
                    Math.Sign(center.Y - obtainedPoint.Y));
                if (optimizationOffset.IsEmpty)
                    return obtainedPoint;
                var possiblePosition = obtainedPoint + optimizationOffset;
                if (!isValidPoint(possiblePosition))
                    return obtainedPoint;
                obtainedPoint = possiblePosition;
            }
        }
    }
}