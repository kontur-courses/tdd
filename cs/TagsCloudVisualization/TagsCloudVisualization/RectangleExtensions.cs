using System.Drawing;

namespace TagsCloudVisualization
{
    public static class RectangleExtensions
    {
        public static Point GetRectangleCenter(this Rectangle rectangle)
        {
            return new Point(
                rectangle.Location.X + rectangle.Width / 2,
                rectangle.Location.Y + rectangle.Height / 2);
        }

        public static Rectangle CenterRectangleLocation(this Rectangle rectangle)
        {
            var center = new Point(
                rectangle.Location.X - rectangle.Width / 2,
                rectangle.Location.Y - rectangle.Height / 2);

            return new Rectangle(center, rectangle.Size);
        }

        public static Rectangle MoveRelativeToPoint(this Rectangle rectangle, Point point)
        {
            var center = new Point(
                rectangle.Location.X + point.X,
                rectangle.Location.Y + point.Y);

            return new Rectangle(center, rectangle.Size);
        }
    }
}