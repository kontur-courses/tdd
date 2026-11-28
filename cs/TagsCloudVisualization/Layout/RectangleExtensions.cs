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

    public static bool IntersectsWithAny(this Rectangle rectangle, IEnumerable<Rectangle> others)
    {
        foreach (var other in others)
        {
            if (rectangle.IntersectsWith(other))
            {
                return true;
            }
        }

        return false;
    }

    public static Rectangle FromCenter(Point center, Size size)
    {
        return new Rectangle(
            center.X - size.Width / 2,
            center.Y - size.Height / 2,
            size.Width,
            size.Height);
    }
}