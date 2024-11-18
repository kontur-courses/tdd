using System.Drawing;

namespace TagsCloudVisualization.Tests
{
    internal static class RectangleExtensions
    {
        public static double GetMaxDistanceFromPointToRectangleAngles(this Rectangle rectangle, Point point)
        {
            var dx = Math.Max(
                Math.Abs(rectangle.X - point.X), Math.Abs(rectangle.X + rectangle.Width - point.X));
            var dy = Math.Max(
                Math.Abs(rectangle.Y - point.Y), Math.Abs(rectangle.Y + rectangle.Height - point.Y));
            return Math.Sqrt(dx * dx + dy * dy);
        }
    }
}