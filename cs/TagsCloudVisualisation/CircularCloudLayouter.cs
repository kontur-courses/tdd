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

        public Point CloudCenter { get; }
        private readonly List<Rectangle> rectangles = new List<Rectangle>();
        private readonly List<CandidatePoint> points = new List<CandidatePoint>();

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangles.Count == 0)
                return CreateAndRegisterRectangle(new Point(rectangleSize / -2) + (Size) CloudCenter, rectangleSize);

            var position = points
                .OrderBy(p => p.CloudCenterDistance)
                .First(p => !IntersectsWithAny(PlaceRectangle(p, rectangleSize)));

            points.Remove(position);
            return CreateAndRegisterRectangle(PlaceRectangle(position, rectangleSize));
        }

        private Rectangle CreateAndRegisterRectangle(Point position, Size size)
        {
            return CreateAndRegisterRectangle(new Rectangle(position, size));
        }

        private Rectangle CreateAndRegisterRectangle(Rectangle rectangle)
        {
            points.AddRange(GetCornersOfRectangle(rectangle));
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