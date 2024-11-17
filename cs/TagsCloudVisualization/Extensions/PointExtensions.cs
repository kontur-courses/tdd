using System.Drawing;

namespace TagsCloudVisualization.Extensions;

public static class PointExtensions
{
    public static double DistanceFromCenter(this Point point, Point center)
    {
        var dx = point.X - center.X;
        var dy = point.Y - center.Y;
        return Math.Sqrt(dx * dx + dy * dy);
    }
    
    public static Point GetDirectionToCenter(this Point center, Rectangle rectangle)
    {
        var rectangleCenter = rectangle.GetCenter();
        
        var directionX = center.X - rectangleCenter.X;
        var directionY = center.Y - rectangleCenter.Y;
        
        return new Point(directionX, directionY);
    }
}