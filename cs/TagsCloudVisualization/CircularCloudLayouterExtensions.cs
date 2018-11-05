using System;
using System.Drawing;

namespace TagsCloudVisualization
{
    static class CircularCloudLayouterExtensions
    {
        public static Rectangle ShiftCoordinatesToCenterRectangle(this Rectangle rectangle)
        {
            var location = new Point(rectangle.X - rectangle.Width / 2, rectangle.Y - rectangle.Height / 2);
            return new Rectangle(location, rectangle.Size);
        }

        public static double GetDistanceToPoint(this Rectangle rectangle, Point point)
        {
            return Math.Sqrt((GetCenter(rectangle).X - point.X) *
                             (GetCenter(rectangle).X - point.X) +
                             (GetCenter(rectangle).Y - point.Y) *
                             (GetCenter(rectangle).Y - point.Y));
        }

        public static Point GetCenter(this Rectangle rectangle)
        {
            return new Point(rectangle.X + rectangle.Width / 2, rectangle.Y + rectangle.Height / 2);
        }
    }
}
