using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualisation
{
    public partial class CircularCloudLayouter : ICircularCloudLayouter
    {
        public CircularCloudLayouter(Point cloudCenter)
        {
            CloudCenter = cloudCenter;
        }

        private readonly Size minRectangleSize = new Size(3, 3);
        public Point CloudCenter { get; }
        private readonly List<Rectangle> rectangles = new List<Rectangle>();

        private readonly ISet<CandidatePoint> points = new SortedSet<CandidatePoint>(Comparer<CandidatePoint>.Create(
            (p1, p2) => p1.CloudCenterDistance.CompareTo(p2.CloudCenterDistance)));

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangles.Count == 0)
                return CreateAndRegisterRectangle(new Point(rectangleSize / -2) + (Size) CloudCenter, rectangleSize);

            // Некрасивый код ниже является оптимизацией и дает хороший буст к скорости
            var pointsToRemove = new List<CandidatePoint>();
            Rectangle? result = null;
            foreach (var point in points)
            {
                var minRectangle = PlaceRectangle(point, minRectangleSize);
                result = PlaceRectangle(point, rectangleSize);
                foreach (var existingRectangle in rectangles)
                {
                    if (IntersectsOrConnected(existingRectangle, minRectangle))
                    {
                        pointsToRemove.Add(point);
                        result = null;
                        break;
                    }

                    if (IntersectsOrConnected(existingRectangle, result.Value))
                    {
                        result = null;
                        break;
                    }
                }

                if (result.HasValue)
                {
                    pointsToRemove.Add(point);
                    result = RegisterRectangle(result.Value);
                    break;
                }
            }

            foreach (var point in pointsToRemove)
                points.Remove(point);

            return result!.Value;
        }

        private Rectangle CreateAndRegisterRectangle(Point position, Size size)
        {
            return RegisterRectangle(new Rectangle(position, size));
        }

        private Rectangle RegisterRectangle(Rectangle rectangle)
        {
            foreach (var point in GetCornersOfRectangle(rectangle))
                points.Add(point);
            rectangles.Add(rectangle);
            return rectangle;
        }

        private ICollection<CandidatePoint> GetCornersOfRectangle(Rectangle rectangle) => new[]
        {
            new CandidatePoint(rectangle.Left, rectangle.Top, CloudCenter, PointDirection.Up),
            new CandidatePoint(rectangle.Right, rectangle.Top, CloudCenter, PointDirection.Right),
            new CandidatePoint(rectangle.Left, rectangle.Bottom, CloudCenter, PointDirection.Left),
            new CandidatePoint(rectangle.Right, rectangle.Bottom, CloudCenter, PointDirection.Down)
        };

        private bool IntersectsWithAny(Rectangle rect) => rectangles.Any(r => IntersectsOrConnected(r, rect));

        private static Rectangle PlaceRectangle(CandidatePoint point, Size size) =>
            new Rectangle(point.Direction switch
            {
                PointDirection.Up => new Point(point.X, point.Y - size.Height - 1),
                PointDirection.Down => new Point(point.X - size.Width, point.Y + 1),
                PointDirection.Left => new Point(point.X - size.Width - 1, point.Y - size.Height),
                PointDirection.Right => new Point(point.X + 1, point.Y),
                _ => throw new ArgumentOutOfRangeException(nameof(point.Direction))
            }, size);

        private static bool IntersectsOrConnected(Rectangle r1, Rectangle r2) =>
            r1.Left <= r2.Right && r2.Left <= r1.Right &&
            r1.Top <= r2.Bottom && r2.Top <= r1.Bottom;
    }
}