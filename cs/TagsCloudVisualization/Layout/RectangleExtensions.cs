using System.Drawing;

namespace TagsCloudVisualization.Layout;

public static class RectangleExtensions
{
    public static PointF GetCenter(this Rectangle rectangle)
    {
        return new PointF(
            rectangle.Left + rectangle.Width / 2f,
            rectangle.Top + rectangle.Height / 2f);
    }
}
