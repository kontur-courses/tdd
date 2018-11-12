using System.Drawing;

namespace TagsCloudVisualization
{
    public static class RectangleExtensions
    {
        public static PointF Center(this Rectangle rectangle)
        {
            var x = rectangle.Left + rectangle.Width / 2.0f;
            var y = rectangle.Top + rectangle.Height / 2.0f;
            return new PointF(x, y);
        }
    }
}
