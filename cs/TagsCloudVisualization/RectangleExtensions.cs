using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public static class RectangleExtensions
    {
        public static bool IntersectsWithRectangles(this Rectangle currentRectangle, List<Rectangle> rectangles)
        {
            foreach (var rectangle in rectangles)
            {
                if (currentRectangle.IntersectsWith(rectangle))
                    return true;
            }
            return false;
        }

        public static double GetDistanceToPoint(this Rectangle rectangle, Point point)
        {
            var rectCenter = new Point(
                rectangle.X + rectangle.Width / 2,
                rectangle.Y + rectangle.Height / 2);
            return Math.Sqrt((point.X - rectangle.X) ^ 2 + (point.Y - rectCenter.Y) ^ 2);
        }

        public static Rectangle GetMovedCopy(this Rectangle rectangle, DirectionToMove direction, int shift)
        {
            var location = new Point(rectangle.X, rectangle.Y);
            switch (direction)
            {
                case DirectionToMove.Up:
                    location.Y -= shift;
                    break;
                case DirectionToMove.Down:
                    location.Y += shift;
                    break;
                case DirectionToMove.Left:
                    location.X -= shift;
                    break;
                case DirectionToMove.Right:
                    location.X += shift;
                    break;
            }
            return new Rectangle(location, rectangle.Size);
        }
    }
}
