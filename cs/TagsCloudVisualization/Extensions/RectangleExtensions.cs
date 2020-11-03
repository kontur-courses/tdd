using System.Drawing;

namespace TagsCloudVisualization.Extensions
{
    public static class RectangleExtensions
    {
        public static Point Center(this Rectangle rectangle)
        {
            return new Point(
                rectangle.X + rectangle.Width / 2,
                rectangle.Y + rectangle.Height / 2);
        }

        public static Point CenterWith(this Point point, Size size)
        {
            return new Point(
                point.X + size.Width / 2,
                point.Y + size.Height / 2);
        }
    }
}
