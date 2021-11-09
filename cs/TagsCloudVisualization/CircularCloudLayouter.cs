using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        public readonly PointF Center;
        public readonly List<RectangleF> Rectangles = new List<RectangleF>();
        private readonly List<PointF> figure = new List<PointF>();

        public CircularCloudLayouter(Point center)
        {
            Center = center;
        }

        public RectangleF PutNextRectangle(Size rectangleSize)
        {
            if (Rectangles.Count == 0)
            {
                var rect = GetRectangle(Center, rectangleSize);
                Rectangles.Add(rect);
                figure.AddRange(rect.GetPoints());
                figure.Add(figure[0]);
                return rect;
            }
            var sortedPoints = GetPlacementLocationsOnMiddleSides()
                .Concat(GetPlacementLocationsOnCorners())
                .OrderBy(p => GetDistance(p.Location, Center));
            foreach (var placement in sortedPoints)
            {
                var rect = TryPlaceRectangle(placement, rectangleSize);
                if (!rect.HasValue) continue;
                AddRectangleToFigure(rect.Value, placement);
                Rectangles.Add(rect.Value);
                return rect.Value;
            }
            throw new Exception();
        }

        private void AddRectangleToFigure(RectangleF rectangle, PlacementLocation location)
        {
            if (location.Placement == Placement.Corner)
                throw new NotImplementedException();
            var index = figure.IndexOf(location.LeftVertex);
            var rectVertexes = rectangle.GetPoints().ToArray();
            var startIndex = 0;
            for (var i = 1; i < 4; i++)
            {
                var oldD = GetDistance(location.LeftVertex, rectVertexes[startIndex]);
                var newD = GetDistance(location.LeftVertex, rectVertexes[i]);
                if (newD < oldD)
                    startIndex = i;
            }
            rectVertexes = rectVertexes.TakeLast(4 - startIndex)
                .Concat(rectVertexes.Take(startIndex))
                .Where(x => !figure.Contains(x))
                .ToArray();
            figure.InsertRange(index + 1, rectVertexes);
        }

        private RectangleF? TryPlaceRectangle(PlacementLocation location, Size size)
        {
            switch (location.Placement)
            {
                case Placement.Corner:
                    throw new NotImplementedException();
                case Placement.VerticalSide:
                    var dx = location.Location.X - Center.X;
                    var offsetX = dx / Math.Abs(dx) * size.Width / 2.0f;
                    return GetRectangle(new PointF( location.Location.X + offsetX, location.Location.Y), size);
                case Placement.HorizontalSide:
                    var dy = location.Location.Y - Center.Y;
                    var offsetY = dy / Math.Abs(dy) * size.Height / 2.0f;
                    return GetRectangle(new PointF(location.Location.X, location.Location.Y + offsetY), size);
                default:
                    return null;
            }
        }

        private RectangleF GetRectangle(PointF center, Size size)
        {
            return new RectangleF(center.X - size.Width / 2.0f, center.Y - size.Height / 2.0f, size.Width, size.Height);
        }

        private IEnumerable<PlacementLocation> GetPlacementLocationsOnMiddleSides()
        {
            for (var i = 0; i < figure.Count - 1; i++)
            {
                var left = figure[i];
                var right = figure[i + 1];
                var location = new PointF();
                var placement = Placement.Corner;
                if (left.X == right.X)
                {
                    if (Center.Y >= Math.Min(left.Y, right.Y) && Center.Y <= Math.Max(left.Y, right.Y))
                    {
                        location = new PointF(left.X, Center.Y);
                        placement = Placement.VerticalSide;
                    }

                }
                else if (left.Y == right.Y)
                {
                    if (Center.X >= Math.Min(left.X, right.X) && Center.X <= Math.Max(left.X, right.X))
                    {
                        location = new PointF(Center.X, left.Y);
                        placement = Placement.HorizontalSide;
                    }
                }
                if (placement != Placement.Corner)
                    yield return new PlacementLocation(left, location, right, placement);
            }
        }

        private IEnumerable<PlacementLocation> GetPlacementLocationsOnCorners()
        {
            yield return new PlacementLocation(figure[^2],
                figure[0], figure[1], Placement.Corner);
            for (var i = 1; i < figure.Count - 1; i++)
            {
                yield return new PlacementLocation(figure[i - 1], figure[i], figure[i + 1], Placement.Corner);
            }
        }

        private PointF ChooseClosestToCenter(PointF a, PointF b)
        {
            return GetDistance(a, Center) > GetDistance(b, Center) ? b : a;
        }

        private double GetDistance(PointF a, PointF b)
        {
            return Math.Sqrt((a.X - b.X)*(a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y));
        }
    }
}