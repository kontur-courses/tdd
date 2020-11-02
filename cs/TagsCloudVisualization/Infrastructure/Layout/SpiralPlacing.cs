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

        public Point GetPlace(Func<Point, bool> isValidPlace)
        {
            int GetRadius(int angle) => angle;
            var angle = 0;

            Point obtainedPlace;
            while (true)
            {
                var possibleLocation = 
                    center + new Size((int) (Math.Sin(angle) * GetRadius(angle)), (int) (Math.Cos(angle) * GetRadius(angle)));
                obtainedPlace = possibleLocation;
                if (isValidPlace(possibleLocation))
                    break;
                angle += angleIncrement;
            }
            var optimisedPlace = CenterUntilValid(obtainedPlace, isValidPlace);
            return optimisedPlace;
        }

        private Point CenterUntilValid(Point obtainedPlace, Func<Point, bool> isValidPlace)
        {
            while (true)
            {
                var targetTrend = new Size(
                    Math.Sign(center.X - obtainedPlace.X),
                    Math.Sign(center.Y - obtainedPlace.Y));
                if (targetTrend == new Size())
                    return obtainedPlace;
                var possiblePosition = obtainedPlace + targetTrend;
                if (!isValidPlace(possiblePosition))
                    break;
                obtainedPlace = possiblePosition;
            }
            return obtainedPlace;
        }
    }
}