using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    internal class CloudCompressor
    {
        private readonly BruteForceNearestFinder nearestFinder = new ();
        private readonly Point compressionPoint;
        private readonly List<Rectangle> cloud;

        public CloudCompressor(Point compressTo, List<Rectangle> cloud)
        {
            compressionPoint = compressTo;
            this.cloud = cloud;
        }

        public Rectangle CompressCloudAfterInsertion(Rectangle insertionRectangle)
        {
            var toCompressionPoint = GetDirectionsForMovingForCompress(insertionRectangle);
            foreach (var direction in toCompressionPoint)
            {
                insertionRectangle.Location = CalculateRectangleLocationAfterCompress(insertionRectangle, direction);
            }
            return insertionRectangle;
        }

        private Point CalculateRectangleLocationAfterCompress(Rectangle forMoving, Direction toCenter)
        {
            var nearest = nearestFinder.FindNearestByDirection(forMoving, toCenter, cloud);
            if (nearest == null) return forMoving.Location;
            var distanceCalculator = nearestFinder.GetMinDistanceCalculatorBy(toCenter);
            var distanceForMove = distanceCalculator(nearest.Value, forMoving);
            return MoveByDirection(forMoving.Location, distanceForMove, toCenter);
        }

        private static Point MoveByDirection(Point forMoving, int distance, Direction whereMoving)
        {
            var factorForDistanceByX = whereMoving switch
            {
                Direction.Left => -1,
                Direction.Right => 1,
                _ => 0
            };
            var factorForDistanceByY = whereMoving switch
            {
                Direction.Top => -1,
                Direction.Bottom => 1,
                _ => 0
            };
            forMoving.X += distance * factorForDistanceByX;
            forMoving.Y += distance * factorForDistanceByY;

            return forMoving;
        }

        private List<Direction> GetDirectionsForMovingForCompress(Rectangle forMoving)
        {
            var directions = new List<Direction>();
            if (forMoving.Bottom < compressionPoint.Y) directions.Add(Direction.Bottom);
            if (forMoving.Left > compressionPoint.X) directions.Add(Direction.Left);
            if (forMoving.Right < compressionPoint.X) directions.Add(Direction.Right);
            if (forMoving.Top > compressionPoint.Y) directions.Add(Direction.Top);
            return directions;
        }
    }
}
