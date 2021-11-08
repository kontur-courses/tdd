using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
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
            else
            {
                var sortedPoints = GetRectanglePositionOnMiddleSides(rectangleSize)
                    .OrderBy(p => GetDistance(p, Center));
                foreach (var point in sortedPoints)
                {
                    var rect = TryPlaceRectangle(point, rectangleSize);
                    if (rect.HasValue)
                    {
                        AddRectangleToFigure(rect.Value);
                        Rectangles.Add(rect.Value);
                        return rect.Value;
                    }
                }
            }
            throw new NotImplementedException();
        }

        private void AddRectangleToFigure(RectangleF rectangle)
        {
            
        }

        private RectangleF? TryPlaceRectangle(PointF center, Size size)
        {
            return GetRectangle(center, size);
        }

        private RectangleF GetRectangle(PointF center, Size size)
        {
            return new RectangleF(center.X - size.Width / 2, center.Y + size.Height / 2, size.Width, size.Width);
        }

        private IEnumerable<PointF> GetRectanglePositionOnMiddleSides(Size size)
        {
            for (var i = 0; i < figure.Count - 1; i++)
            {
                var a = figure[i];
                var b = figure[i + 1];
                if (a.X == b.X)
                {
                    if (Center.Y >= Math.Min(a.Y, b.Y) && Center.Y <= Math.Max(a.Y, b.Y))
                    {
                        if (a.X > Center.X)
                            yield return new PointF(a.X + size.Width / 2, Center.Y);
                        else
                            yield return new PointF(a.X - size.Width / 2, Center.Y);
                    }
                }
                else if (a.Y == b.Y)
                    if (Center.X >= Math.Min(a.X, b.X) && Center.X <= Math.Max(a.X, b.X))
                    {
                        if (a.Y > Center.Y)
                            yield return new PointF(Center.X, a.Y + size.Height / 2);
                        else
                            yield return new PointF(Center.X, a.Y - size.Height / 2);
                    }
            }
        }

        private PointF ChooseClosestToCenter(PointF a, PointF b)
        {
            return GetDistance(a, Center) > GetDistance(b, Center) ? b : a;
        }

        private double GetDistance(PointF a, PointF b)
        {
            return Math.Sqrt(((double)a.X) * b.X + ((double)a.Y) * b.Y);
        }
    }
}