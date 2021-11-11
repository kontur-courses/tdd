using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization.Internals
{
    internal static class CircularCloudLayouterInternals
    {
        public static IEnumerable<SlottedAnchor> FilterForFilledSlots(this IEnumerable<SlottedAnchor> anchors, Direction directionToFilter)
        {
            return anchors.Where(x => x.FilledSlots != directionToFilter);
        }

        public static IEnumerable<(Point point, Direction direction)> GetAllValidSlots(this SlottedAnchor anchor)
        {
            return anchor.GetEmptySlots().Select(direction => (anchor.GetSlotAt(direction), direction));
        }

        public static bool IntersectsWithAny(this Rectangle rectangle, IEnumerable<SlottedAnchor> toCheck)
        {
            return toCheck.Any(x => x.IntersectsWith(rectangle));
        }

        public static Rectangle GetRectangleAt(this SlottedAnchor anchor, Direction direction, Size size)
        {
            var location = GetLocationForSlotAt(anchor.GetSlotAt(direction), direction, size);
            return new(location, size);
        }

        private static Point GetLocationForSlotAt(this SlottedAnchor anchor, Direction direction, Size size)
        {
            var horizontalOffset = (anchor.Width - size.Width) / 2;
            var verticalOffset = (anchor.Height - size.Height) / 2;
            return direction switch
            {
                Direction.Top => new Point(anchor.Left + horizontalOffset, anchor.Top - size.Height),
                Direction.Bottom => new Point(anchor.Left + horizontalOffset, anchor.Bottom),
                Direction.Right => new Point(anchor.Right, anchor.Top + verticalOffset),
                Direction.Left => new Point(anchor.Left, anchor.Top + verticalOffset),
                _ => throw new ArgumentException($"Invalid direction: {direction}"),
            };
        }

        public static Point GetLocationForSlotAt(Point slotLocation, Direction direction, Size size)
        {
            var horizontalOffset = -size.Width / 2;
            var verticalOffset = -size.Height / 2;
            return direction switch
            {
                Direction.Top => slotLocation + new Size(horizontalOffset, -size.Height),
                Direction.Bottom => slotLocation + new Size(horizontalOffset, 0),
                Direction.Right => slotLocation + new Size(0, verticalOffset),
                Direction.Left => slotLocation + new Size(-size.Width, verticalOffset),
                _ => throw new ArgumentException($"Invalid direction: {direction}"),
            };
        }
    }
}
