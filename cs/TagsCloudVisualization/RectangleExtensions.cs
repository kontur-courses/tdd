using System.Drawing;

namespace TagsCloudVisualization
{
    public static class RectangleExtensions
    {
        public static Point GetCenter(this Rectangle rectangle)
        {
            var x = rectangle.X + rectangle.Width / 2;
            var y = rectangle.Y + rectangle.Height / 2;
            return new Point(x, y);
        }
    }
}