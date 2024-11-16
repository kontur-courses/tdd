using System.Drawing;

namespace TagsCloudVisualization.Extensions;

public static class PointExtensions
{
    public static Point Subtract(this Point point1, Point point2) => 
        new(point1.X - point2.X, point1.Y - point2.Y);
    
    public static Point Abs(this Point point) => 
        new(Math.Abs(point.X), Math.Abs(point.Y));
}