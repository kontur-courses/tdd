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
        private readonly List<PlacedRectangle> rectangles = new List<PlacedRectangle>();

        private readonly OrderedList<CandidatePoint, double> points =
            new OrderedList<CandidatePoint, double>(p => p.CloudCenterDistance);

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangles.Count == 0)
                return Rectangle(new Point(rectangleSize / -2), rectangleSize);

            var position = points.First(p => !IsIntersectsWithAny(p.GetRectangle(rectangleSize)));
            points.Remove(position);

            return Rectangle(position, rectangleSize);
        }

        private PlacedRectangle Rectangle(Point position, Size size)
        {
            var rectangle = new PlacedRectangle(position, size, CloudCenter);
            foreach (var point in rectangle.Corners.Where(p => !IsIntersectsWithAny(p.GetRectangle(size))))
                points.Add(point);
            rectangles.Add(rectangle);
            return rectangle;
        }

        private CandidatePoint Point(int x, int y, PlacedRectangle parent, PointDirection direction) =>
            new CandidatePoint(x, y, CloudCenter, parent, direction);

        private bool IsIntersectsWithAny(Rectangle rectangle) => rectangles.Any(r => r.Intersected(rectangle));
    }
}