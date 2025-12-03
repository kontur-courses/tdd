using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagCloud
{
    public class CircularCloudLayouter
    {
        private Point Center { get; }
        private readonly List<Rectangle> rectangles = new List<Rectangle>();
        public IReadOnlyList<Rectangle> Rectangles => rectangles.AsReadOnly();

        private readonly IPointGetter pointGetter;

        public CircularCloudLayouter(Point center, IPointGetter pointGetter)
        {
            this.Center = center;
            this.pointGetter = pointGetter;
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
                throw new ArgumentException("Стороны должны иметь положительный размер", nameof(rectangleSize));

            if (!rectangles.Any())
            {
                var location = new Point(Center.X - rectangleSize.Width / 2, Center.Y - rectangleSize.Height / 2);
                var rect = new Rectangle(location, rectangleSize);
                rectangles.Add(rect);
                return rect;
            }

            for (int iter = 0; iter < 200000; iter++)
            {
                var p = pointGetter.GetNextPoint();
                var topLeft = new Point(p.X - rectangleSize.Width / 2, p.Y - rectangleSize.Height / 2);
                var candidate = new Rectangle(topLeft, rectangleSize);

                if (!TagCloudUtils.IntersectsAny(candidate, rectangles))
                {
                    candidate = MoveCloserToCenter(candidate);
                    rectangles.Add(candidate);
                    return candidate;
                }
            }

            throw new InvalidOperationException("Превзойдено ограничение итераций");
        }

        private Rectangle MoveCloserToCenter(Rectangle rect)
        {
            var candidate = rect;
            var moved = true;
            while (moved)
            {
                moved = false;
                var movedCandidate = TagCloudUtils.MoveVerticalTowardsCenter(candidate, Center);
                if (!TagCloudUtils.IntersectsAny(movedCandidate, rectangles) && TagCloudUtils.DistanceToCenter(movedCandidate, Center) < TagCloudUtils.DistanceToCenter(candidate, Center))
                {
                    candidate = movedCandidate;
                    moved = true;
                }
                movedCandidate = TagCloudUtils.MoveHorizontalTowardsCenter(candidate, Center);
                if (!TagCloudUtils.IntersectsAny(movedCandidate, rectangles) && TagCloudUtils.DistanceToCenter(movedCandidate, Center) < TagCloudUtils.DistanceToCenter(candidate, Center))
                {
                    candidate = movedCandidate;
                    moved = true;
                }
            }
            return candidate;
        }
    }
}
