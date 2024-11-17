using System.Drawing;

namespace TagsCloudVisualization.Extensions;

public static class RectangleExtensions
{
    public static double DistanceToCenter(this Rectangle rectangle, Point center)
    {
        var rectangleCenter = rectangle.GetCenter();
        
        var dx = rectangleCenter.X - center.X;
        var dy = rectangleCenter.Y - center.Y;
        return Math.Sqrt(dx * dx + dy * dy);
    }
    
    public static Point GetCenter(this Rectangle rectangle)
    {
        return new Point(
            rectangle.Left + rectangle.Width / 2,
            rectangle.Top + rectangle.Height / 2);
    }
}