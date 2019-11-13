using System.Drawing;

namespace TagsCloudVisualization
{
    public static class RectangleUtils
    {
        public static Point GetCenter(this Rectangle rect)
        {
            return new Point(
                rect.X + rect.Width / 2,
                rect.Y + rect.Height / 2);
        }
        
        public static double DistanceToPoint(this Rectangle rect, Point point)
        {
            return rect.GetCenter().DistanceTo(point);
        }

        public static Rectangle ShiftLocation(this Rectangle rect, Point shift)
        {
            return new Rectangle(new Point(rect.X + shift.X, rect.Y + shift.Y), rect.Size);
        }
    }
}