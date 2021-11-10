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
        public Polygon figure;

        public CircularCloudLayouter(Point center)
        {
            Center = center;
        }

        public RectangleF PutNextRectangle(Size rectangleSize)
        {
            if (Rectangles.Count == 0)
            {
                var rectangle = GetRectangle(Center, rectangleSize);
                Rectangles.Add(rectangle);
                figure = new Polygon(rectangle);
                return rectangle;
            }
            var sortedLocations = GetPlacementLocationsOnMiddleSides()
                .Concat(GetPlacementLocationsOnCorners())
                .OrderBy(p => GetDistance(p.Location, Center));
            foreach (var placement in sortedLocations)
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
            var index = figure.Vertexes.IndexOf(location.LeftVertex);
            var rectVertexes = rectangle.GetPoints().ToList();
            var startIndex = 0;
            if (location.Placement == Placement.Corner)
            {
                startIndex = rectVertexes.FindIndex(p => p == location.Location);
                figure.Vertexes.Remove(location.Location);
                rectVertexes.RemoveAt(startIndex);
            }
            else
            {
                for (var i = 1; i < 4; i++)
                {
                    var oldD = GetDistance(location.LeftVertex, rectVertexes[startIndex]);
                    var newD = GetDistance(location.LeftVertex, rectVertexes[i]);
                    if (newD < oldD)
                        startIndex = i;
                }
            }
            figure.Vertexes.InsertRange((index + 1) % figure.Vertexes.Count, rectVertexes.Skip(startIndex)
                                                                .Concat(rectVertexes.Take(startIndex)));
            figure.Normalize();
        }

        

        private RectangleF? TryPlaceRectangle(PlacementLocation location, Size size)
        {
            var offsetX = 0f;
            var offsetY = 0f;
            switch (location.Placement)
            {
                
                case Placement.Corner:
                    offsetX = Math.Sign(location.Location.X - Center.X) * size.Width / 2.0f;
                    offsetY = Math.Sign(location.Location.Y - Center.Y) * size.Height / 2.0f;
                    break;
                case Placement.VerticalSide:
                    offsetX = Math.Sign(location.Location.X - Center.X) * size.Width / 2.0f;
                    break;
                case Placement.HorizontalSide:
                    offsetY = Math.Sign(location.Location.Y - Center.Y) * size.Height / 2.0f;
                    break;
            }
            return GetRectangle(new PointF( location.Location.X + offsetX, location.Location.Y  + offsetY), size);
        }

        private RectangleF GetRectangle(PointF center, Size size)
        {
            return new RectangleF(center.X - size.Width / 2.0f, center.Y - size.Height / 2.0f, size.Width, size.Height);
        }

        private IEnumerable<PlacementLocation> GetPlacementLocationsOnMiddleSides()
        {
            foreach (var (left, right) in figure.GetSegments())
            {
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
            yield return new PlacementLocation(figure.Vertexes[^1],
                figure.Vertexes[0], figure.Vertexes[1], Placement.Corner);
            for (var i = 1; i < figure.Vertexes.Count - 1; i++)
            {
                yield return new PlacementLocation(figure.Vertexes[i - 1], figure.Vertexes[i],
                    figure.Vertexes[i + 1], Placement.Corner);
            }
            yield return new PlacementLocation(figure.Vertexes[^2], figure.Vertexes[^1],
                figure.Vertexes[0], Placement.Corner);
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