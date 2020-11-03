using System.Collections.Generic;
using System.Diagnostics;
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
                return Rectangle(new Rectangle(new Point(rectangleSize / -2), rectangleSize));

            var position = points.First(p => !IsIntersectsWithAny(p.PlaceRectangle(rectangleSize)));
            points.Remove(position);

            return Rectangle(position.PlaceRectangle(rectangleSize));
        }

        private PlacedRectangle Rectangle(Rectangle rect)
        {
            var placedRect = new PlacedRectangle(rect, CloudCenter);
            //                                                                                        TODO wat??
            foreach (var point in placedRect.Corners.Where(p => !IsIntersectsWithAny(p.PlaceRectangle(rect.Size))))
            {
                points.Add(point);
            }

            rectangles.Add(placedRect);
            return placedRect;
        }

        private CandidatePoint Point(int x, int y, PlacedRectangle parent, PointDirection direction) =>
            new CandidatePoint(x, y, CloudCenter, parent, direction);

        private bool IsIntersectsWithAny(Rectangle rectangle) => rectangles.Any(r => r.Intersected(rectangle));
    }
}