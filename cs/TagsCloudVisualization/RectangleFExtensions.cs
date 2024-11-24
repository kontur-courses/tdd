using System.Drawing;

namespace TagsCloudVisualization;

public static class RectangleFExtensions
{
    public static RectangleF GetRectangleFWithCenterIn(PointF center, SizeF size)
        => new(center.X - size.Width / 2, center.Y + size.Height / 2, size.Width, size.Height);

    public static PointF GetCenter(this RectangleF rectangle)
        => new(rectangle.X + rectangle.Width / 2, rectangle.Y - rectangle.Height / 2);

    public static float DistanceTo(this RectangleF rectangle, RectangleF other)
    {
        var rectCenter = GetCenter(rectangle);
        var otherCenter = GetCenter(rectangle);
        var deltaX = rectCenter.X - otherCenter.X;
        var deltaY = rectCenter.Y - otherCenter.Y;

        return MathF.Sqrt(deltaX * deltaX + deltaY * deltaY);
    }
}