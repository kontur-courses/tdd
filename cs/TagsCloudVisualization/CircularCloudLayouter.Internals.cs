using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization.Internals
{
    internal static class CircularCloudLayouter
    {
        public static IEnumerable<SlottedAnchor> FilterForFilledSlots(this IEnumerable<SlottedAnchor> anchors, Direction directionToFilter)
        {
            return anchors.Where(x => x.FilledSlots != directionToFilter);
        }

        public static Rectangle GetRectangleAt(this SlottedAnchor anchor, Direction direction, Size size)
        {
            var rectangle = new Rectangle(anchor.GetLocationForSlotAt(direction, size), size);
            return rectangle;
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
                Direction.Left => new Point(anchor.Left - size.Width, anchor.Top + verticalOffset),
                _ => throw new ArgumentException($"Invalid direction: {direction}"),
            };
        }
    }
}
