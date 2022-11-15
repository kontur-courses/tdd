using System.Drawing;

namespace TagsCloudVisualization
{
    public static class RectangleExtensions
    {
        public static Point GetCenter(this Rectangle rectangle)
        {
            var startPoint = rectangle.Location;
            return new Point(startPoint.X + rectangle.Width / 2, startPoint.Y + rectangle.Height / 2);
        }
    }
}