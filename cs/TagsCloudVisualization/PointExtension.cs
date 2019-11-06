using System.Drawing;

namespace TagsCloudVisualization
{
    internal static class PointExtension
    {
        internal static Point Add(this Point point, Point other)
        {
            return new Point(point.X + other.X, point.Y + other.Y);
        }
    }
}
