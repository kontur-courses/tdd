using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        public readonly PointF Center;
        public readonly List<RectangleF> Rectangles = new List<RectangleF>();
        public Polygon Figure;

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
                Figure = new Polygon(rectangle);
                return rectangle;
            }
            var sortedLocations = GetPlacementLocationsOnMiddleSides()
                .Concat(GetPlacementLocationsOnCorners())
                .OrderBy(p => p.Location.DistanceTo(Center));
            foreach (var placement in sortedLocations)
            {
                if (!TryPlaceRectangle(placement, rectangleSize, out var rectangle)) 
                    continue;
                AddRectangleToFigure(rectangle, placement);
                Rectangles.Add(rectangle);
                return rectangle;
            }
            throw new Exception();
        }

        private void AddRectangleToFigure(RectangleF rectangle, PlacementLocation location)
        {
            var index = Figure.Vertexes.IndexOf(location.LeftVertex);
            var rectVertexes = rectangle.GetPoints().ToList();
            var startIndex = 0;
            if (location.Placement == Placement.Corner)
            {
                startIndex = rectVertexes.FindIndex(p => p == location.Location);
                Figure.Vertexes.Remove(location.Location);
                rectVertexes.RemoveAt(startIndex);
            }
            else
            {
                for (var i = 1; i < 4; i++)
                {
                    var oldD = location.LeftVertex.DistanceTo(rectVertexes[startIndex]);
                    var newD = location.LeftVertex.DistanceTo(rectVertexes[i]);
                    if (newD < oldD)
                        startIndex = i;
                }
            }
            if (index + 1 > Figure.Vertexes.Count)
                Figure.Vertexes.AddRange(rectVertexes.Skip(startIndex)
                    .Concat(rectVertexes.Take(startIndex)));
            else
                Figure.Vertexes.InsertRange((index + 1) % Figure.Vertexes.Count, rectVertexes.Skip(startIndex)
                                                                .Concat(rectVertexes.Take(startIndex)));
            Figure.Normalize();
        }

        

        private bool TryPlaceRectangle(PlacementLocation location, Size size, out RectangleF rectangle)
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
            var tempRectangle = GetRectangle(new PointF( location.Location.X + offsetX, location.Location.Y  + offsetY), size);
            rectangle = tempRectangle;
            return Rectangles.All(r =>
            {
                r.Intersect(tempRectangle);
                return r.IsEmpty(0.01);
            });
        }

        private RectangleF GetRectangle(PointF center, Size size)
        {
            return new RectangleF(center.X - size.Width / 2.0f, center.Y - size.Height / 2.0f, size.Width, size.Height);
        }

        private IEnumerable<PlacementLocation> GetPlacementLocationsOnMiddleSides()
        {
            foreach (var (left, right) in Figure.GetSegments())
            {
                var location = new PointF();
                var placement = Placement.Corner;
                if (Math.Abs(left.X - right.X) < 0.01)
                {
                    if (Center.Y >= Math.Min(left.Y, right.Y) && Center.Y <= Math.Max(left.Y, right.Y))
                    {
                        location = new PointF(left.X, Center.Y);
                        placement = Placement.VerticalSide;
                    }

                }
                else if (Math.Abs(left.Y - right.Y) < 0.01)
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
            yield return new PlacementLocation(Figure.Vertexes[^1],
                Figure.Vertexes[0], Figure.Vertexes[1], Placement.Corner);
            for (var i = 1; i < Figure.Vertexes.Count - 1; i++)
            {
                yield return new PlacementLocation(Figure.Vertexes[i - 1], Figure.Vertexes[i],
                    Figure.Vertexes[i + 1], Placement.Corner);
            }
            yield return new PlacementLocation(Figure.Vertexes[^2], Figure.Vertexes[^1],
                Figure.Vertexes[0], Placement.Corner);
        }
    }
}