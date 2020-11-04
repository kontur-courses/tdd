using System.Drawing;

namespace TagsCloudVisualization.Extensions
{
    public static class PointExtensions
    {
        public static Point CenterWith(this Point point, Size size)
        {
            return new Point(
                point.X + size.Width / 2,
                point.Y + size.Height / 2);
        }
    }
}