using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class CircularCloudMaker
    {
        public readonly PointF Center;
        private readonly List<RectangleF> rectangles = new List<RectangleF>();
        private readonly HashSet<PointF> vertexes = new HashSet<PointF>();
        private readonly HashSet<PointF> pointsOnSides = new HashSet<PointF>();

        public IReadOnlyList<RectangleF> Rectangles => rectangles;
        public double Radius => vertexes.Select(p => p.DistanceTo(Center)).Max();

        public CircularCloudMaker(Point center)
        {
            Center = center;
        }

        public RectangleF PutNextRectangle(Size rectangleSize)
        {
            if (rectangles.Count == 0)
            {
                var rectangle = GetRectangle(Center, rectangleSize);
                rectangles.Add(rectangle);
                foreach (var vertex in rectangle.GetPoints())
                {
                    vertexes.Add(vertex);
                }
                foreach (var segment in rectangle.GetSegments())
                {
                    pointsOnSides.Add(new PointF((segment.Item1.X + segment.Item2.X) / 2,
                        (segment.Item1.Y + segment.Item2.Y) / 2));
                }
                return rectangle;
            }
            foreach (var rectanglePlacement in vertexes.Concat(pointsOnSides)
                .Select(p => Tuple.Create(GetRectangleCenter(p, rectangleSize), p))
                .OrderBy(p => Center.DistanceTo(p.Item1)))
            {
                var rectangle = GetRectangle(rectanglePlacement.Item1, rectangleSize);
                if (!RectangleCanBePlaced(rectangle, rectanglePlacement.Item2)) continue;
                rectangles.Add(rectangle);
                AddRectangle(rectangle, rectanglePlacement.Item2);
                return rectangle;
            }
            return default;
        }

        private void AddRectangle(RectangleF rectangle, PointF location)
        {
            var points = rectangle.GetPoints().ToList();
            if (pointsOnSides.Contains(location))
            {
                pointsOnSides.Remove(location);
                pointsOnSides.Add(OffsetPointFromCenter(location, rectangle.Width, rectangle.Height));
            }
            else
            {
                points.Remove(location);
                vertexes.Remove(location);
            }

            var tempSize = new Size(1, 1);
            points.ForEach(p => vertexes.Add(p));
        }

        private PointF OffsetPointFromCenter(PointF point, float dx, float dy)
        {
            var offsetX = 0f;
            var offsetY = 0f;
            if (Math.Abs(point.X - Center.X) > 0.01)
                offsetX = Math.Sign(point.X - Center.X) * dx;
            if (Math.Abs(point.Y - Center.Y) > 0.01 )
                offsetY = Math.Sign(point.Y - Center.Y) * dy;
            return new PointF(point.X + offsetX, point.Y + offsetY);
        }
        
        private PointF GetRectangleCenter(PointF placementLocation, Size size)
        {
            return OffsetPointFromCenter(placementLocation, size.Width / 2.0f, size.Height / 2.0f);
        }

        private bool RectangleCanBePlaced(RectangleF rectangle, PointF location)
        {
            var result = rectangles
                .All(r => !r.IntersectsWith(rectangle));
            if (result) 
                return true;
            var tempSize = new Size(1, 1);
            var possibleRect = GetRectangle(GetRectangleCenter(location, tempSize), tempSize);
            if (rectangles.Any(r => r.IntersectsWith(possibleRect)))
                vertexes.Remove(location);
            return false;
        }

        private RectangleF GetRectangle(PointF center, Size size)
        {
            return new RectangleF(center.X - size.Width / 2.0f, center.Y - size.Height / 2.0f, size.Width, size.Height);
        }
    }
}