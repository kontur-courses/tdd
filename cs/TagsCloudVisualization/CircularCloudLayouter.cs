using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        private class SlottedAnchor
        {
            public readonly Rectangle Rectangle;
            public Direction FilledSlots;

            public SlottedAnchor(Rectangle rectangle, Direction filledSlots)
            {
                Rectangle = rectangle;
                FilledSlots = filledSlots;
            }

            public int Left => Rectangle.Left;
            public int Right => Rectangle.Right;
            public int Top => Rectangle.Top;
            public int Bottom => Rectangle.Bottom;
            public int Width => Rectangle.Width;
            public int Height => Rectangle.Height;
        }

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
            var (_, data) = anchors
                .Where(x => x.FilledSlots != Direction.All)
                .SelectMany(currAnchor => GetPossiblePositions(currAnchor, nextSize)
                    .Where(pendingPoint => (pendingPoint.direction & currAnchor.FilledSlots) == Direction.None))
                .Select(x => (parent: x.anchor, rectangle: new Rectangle(x.point, nextSize), x.direction))
                .Select(x => (x.parent, current: new SlottedAnchor(x.rectangle, x.direction.GetReversed())))
                .Select(x => (distance: x.current.Rectangle.GetCenter().DistanceTo(Center), data: x))
                .Where(rectangle => anchors
                    .Select(x => x.Rectangle)
                    .All(x => !x.IntersectsWith(rectangle.data.current.Rectangle)))
                .MinBy(x => x.distance);
            var (parent, current) = data;
            parent.FilledSlots |= current.FilledSlots.GetReversed();
            return current;
        }

        private static IEnumerable<(SlottedAnchor anchor, Point point, Direction direction)> GetPossiblePositions(SlottedAnchor anchor, Size size)
        {
            yield return (anchor,
                new Point(anchor.Left + (anchor.Width - size.Width) / 2, anchor.Top - size.Height),
                Direction.Top);
            yield return (anchor,
                new Point(anchor.Left + (anchor.Width - size.Width) / 2, anchor.Bottom),
                Direction.Bottom);
            yield return (anchor,
                new Point(anchor.Right, anchor.Top + (anchor.Height - size.Height) / 2),
                Direction.Right);
            yield return (anchor,
                new Point(anchor.Left - size.Width, anchor.Top + (anchor.Height - size.Height) / 2),
                Direction.Left);
        }
    }
}
