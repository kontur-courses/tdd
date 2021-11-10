using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagsCloudVisualization.Internals;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        public readonly Point Center;
        private readonly List<SlottedAnchor> anchors = new();
        public CircularCloudLayouter(Point center)
        {
            Center = center;
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Width == 0 || rectangleSize.Height == 0)
                throw new ArgumentOutOfRangeException(nameof(rectangleSize));
            if (rectangleSize.Width < 0 || rectangleSize.Height < 0)
                rectangleSize = rectangleSize.Abs();
            SlottedAnchor anchor;
            if (anchors.Count == 0)
            {
                var location = rectangleSize / 2 * -1;
                anchor = new(new Rectangle(new Point(location + new Size(Center)), rectangleSize), Direction.None);
            }
            else
            {
                anchor = CreateNextRectangle(rectangleSize);
            }

            anchors.Add(anchor);

            return anchor.Rectangle;
        }

        private SlottedAnchor CreateNextRectangle(Size nextSize)
        {
            var data = anchors.FilterForFilledSlots(Direction.All)
                .SelectMany(anchor => GetAllValidSlots(anchor, nextSize).Select(current => (parent: anchor, current)))
                .MinBy(x => x.current.GetCenter().DistanceTo(Center));
            var (parent, current) = data;
            parent.FillDirection(current.FilledSlots.GetReversed());
            return current;
        }

        private IEnumerable<SlottedAnchor> GetAllValidSlots(SlottedAnchor anchor, Size size)
        {
            foreach (var (direction, rectangle) in anchor.GetEmptySlots().Select(direction => (direction, anchor.GetRectangleAt(direction, size))))
            {
                var hasIntersection = anchors.Any(x => x.IntersectsWith(rectangle));
                if (hasIntersection)
                    continue;
                yield return new SlottedAnchor(rectangle, direction.GetReversed());
            }
        }
    }
}
