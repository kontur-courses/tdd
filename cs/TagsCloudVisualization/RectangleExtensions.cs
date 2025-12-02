using System.Drawing;

namespace TagsCloudVisualization;

public static class RectangleExtensions
{
    public static PointF GetCenter(this Rectangle rectangle)
    {
        var rectangleCenterX = rectangle.Location.X + rectangle.Size.Width / 2f;
        var rectangleCenterY = rectangle.Location.Y + rectangle.Height / 2f;
        
        return new PointF(rectangleCenterX, rectangleCenterY);
    }
    
    public static bool IsRectanglesIntersect(this Rectangle rectangle, Point centerSize, Size size)
    {
        var left = centerSize.X - size.Width / 2;
        var top = centerSize.Y - size.Height / 2;
        var right = left + size.Width;
        var bottom = top + size.Height;

        return !(rectangle.Right < left  ||
                 rectangle.Bottom < top  ||
                 rectangle.Left > right  ||
                 rectangle.Top > bottom);
    }
}