using System.Drawing;

namespace TagsCloudVisualization
{
    public static class RectangleExtensions
    {
        public static Point GetCenter(this Rectangle rectangle) =>
            new Point(rectangle.Left + rectangle.Width / 2, rectangle.Top + rectangle.Height / 2);
    }
}