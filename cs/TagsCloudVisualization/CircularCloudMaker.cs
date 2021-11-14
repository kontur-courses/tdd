using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class CircularCloudMaker
    {
        private const double FloatDelta = 0.01;
        
        public readonly PointF Center;
        private readonly List<RectangleF> rectangles = new List<RectangleF>();
        private readonly HashSet<PointF> placementLocations = new HashSet<PointF>();

        public IReadOnlyList<RectangleF> Rectangles => rectangles;
        public double Radius => placementLocations.Max(p => GetDistanceBetween(p, Center));

        public CircularCloudMaker(Point center)
        {
            Center = center;
        }

        public RectangleF PutRectangle(Size rectangleSize)
        {
            return rectangles.Count == 0 ? PutFirstRectangle(rectangleSize) : PutNextRectangle(rectangleSize);
        }

        private RectangleF PutFirstRectangle(Size rectangleSize)
        {
            var firstRectangle = CreateRectangle(Center, rectangleSize);
            AddRectanglePointsToPlacementLocations(firstRectangle, Center);
            return firstRectangle;
        }

        private RectangleF PutNextRectangle(Size rectangleSize)
        {
            foreach (var (center, placement) in placementLocations
                .Select(p => (rectangleCenter: GetRectangleCenter(p, rectangleSize), placementLocation: p))
                .OrderBy(p => GetDistanceBetween(Center, p.rectangleCenter)))
            {
                var rectangle = CreateRectangle(center, rectangleSize);
                if (!RectangleCanBePlaced(rectangle, placement)) continue;
                AddRectanglePointsToPlacementLocations(rectangle, placement);
                return rectangle;
            }
            return default;
        }

        private void AddRectanglePointsToPlacementLocations(RectangleF rectangle, PointF placement)
        {
            var points = GetPoints(rectangle).ToList();
            points.Remove(placement);
            placementLocations.Remove(placement);
            rectangles.Add(rectangle);
            foreach (var point in points)
            {
                placementLocations.Add(point);
                placementLocations.Add(new PointF(point.X, Center.Y));
                placementLocations.Add(new PointF(Center.X, point.Y));
            }
        }

        private PointF GetRectangleCenter(PointF placement, Size rectangleSize)
        {
            var offsetX = 0f;
            var offsetY = 0f;
            if (Math.Abs(placement.X - Center.X) > FloatDelta)
                offsetX = Math.Sign(placement.X - Center.X) * rectangleSize.Width / 2.0f;
            if (Math.Abs(placement.Y - Center.Y) > FloatDelta )
                offsetY = Math.Sign(placement.Y - Center.Y) * rectangleSize.Height / 2.0f;
            return new PointF(placement.X + offsetX, placement.Y + offsetY);
        }

        private bool RectangleCanBePlaced(RectangleF rectangle, PointF placement)
        {
            var canBePlaced = rectangles
                .All(r => !r.IntersectsWith(rectangle));
            if (canBePlaced)
                return true;
            var minSize = new Size(1, 1);
            var possibleRectangleCenter = GetRectangleCenter(placement, minSize);
            var possibleRect = CreateRectangle(possibleRectangleCenter, minSize);
            if (rectangles.Any(r => r.IntersectsWith(possibleRect)))
                placementLocations.Remove(placement);
            return false;
        }

        private static RectangleF CreateRectangle(PointF center, Size size)
        {
            return new RectangleF(center.X - size.Width / 2.0f, center.Y - size.Height / 2.0f, size.Width, size.Height);
        }
        
        private static double GetDistanceBetween(PointF point, PointF other)
        {
            return Math.Sqrt((point.X - other.X)*(point.X - other.X) + (point.Y - other.Y) * (point.Y - other.Y));
        }
        
        private static IEnumerable<PointF> GetPoints(RectangleF rectangle)
        {
            yield return rectangle.Location;
            yield return new PointF(rectangle.Right, rectangle.Top);
            yield return new PointF(rectangle.Right, rectangle.Bottom);
            yield return new PointF(rectangle.Left, rectangle.Bottom);
        }
    }
}