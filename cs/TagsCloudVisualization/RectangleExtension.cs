using System.Drawing;

namespace TagsCloudVisualization
{
    public static class RectangleExtension
    {
        public static Point GetCenter(this Rectangle rectangle) =>
            new Point(
                rectangle.X + rectangle.Width / 2,
                rectangle.Y + rectangle.Height / 2
            );
    }
}
