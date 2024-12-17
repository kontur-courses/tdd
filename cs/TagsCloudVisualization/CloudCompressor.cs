using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    internal class CloudCompressor
    {
        private readonly Point compressionPoint;
        private readonly List<Rectangle> cloud;
        private int minDistanceForMoving = 1;

        public CloudCompressor(Point compressTo, List<Rectangle> cloud)
        {
            compressionPoint = compressTo;
            this.cloud = cloud;
        }


        public Rectangle CompressCloudAfterInsertion(Rectangle forInsertion)
        {
            var toCompressionPoint = GetDirectionsForMovingForCompress(forInsertion);
            var beforeIntersection = forInsertion;
            var prevDirectionHasIntersection = false;
            for (var i = 0; i < toCompressionPoint.Count; i++)
            {
                var direction = toCompressionPoint[i];

                while (!forInsertion.IntersectedWithAnyFrom(cloud)
                && !IsIntersectCompressionPointAxis(direction, forInsertion))
                {
                    beforeIntersection = forInsertion;
                    var distance = GetDistanceForMoving(forInsertion, direction);
                    forInsertion.Location = MoveByDirection(forInsertion.Location, 
                        distance == 0 ? minDistanceForMoving : distance, direction);
                }

                var wasIntersection = !IsIntersectCompressionPointAxis(direction, forInsertion);
                if (!prevDirectionHasIntersection && wasIntersection)
                {
                    forInsertion = beforeIntersection;
                    prevDirectionHasIntersection = true;
                }
            }
            return beforeIntersection;
        }


        private int GetDistanceForMoving(Rectangle forMoving, Direction toCompressionPoint)
        {
            var nearest = BruteForceNearestFinder.FindNearestByDirection(forMoving, toCompressionPoint, cloud);
            if (nearest == null) return minDistanceForMoving;
            return BruteForceNearestFinder.CalculateMinDistanceBy(toCompressionPoint, nearest.Value, forMoving);
        }

        private bool IsIntersectCompressionPointAxis(Direction toCompressionPoint, Rectangle current)
        {
            return toCompressionPoint switch
            {
                Direction.Left => current.Left < compressionPoint.X,
                Direction.Right => current.Right > compressionPoint.X,
                Direction.Top => current.Top < compressionPoint.Y,
                Direction.Bottom => current.Bottom > compressionPoint.Y
            };
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
            if (forMoving.Bottom <= compressionPoint.Y) directions.Add(Direction.Bottom);
            if (forMoving.Top >= compressionPoint.Y) directions.Add(Direction.Top);
            if (forMoving.Left >= compressionPoint.X) directions.Add(Direction.Left);
            if (forMoving.Right <= compressionPoint.X) directions.Add(Direction.Right);

            return directions;
        }
    }
}
