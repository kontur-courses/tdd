using System.Drawing;

namespace TagsCloudVisualization.Extensions;

public static class RectangleExtension
{
    public static Rectangle CreateRectangleWithCenter(this Rectangle rectangle, Point center, Size rectangleSize)
    {
        var x = center.X - rectangleSize.Width / 2;
        var y = center.Y - rectangleSize.Height / 2;
        return new Rectangle(x, y, rectangleSize.Width, rectangleSize.Height);
    }

    public static double GetDistanceToMostRemoteCorner(this Rectangle rectangle, Point startingPoint)
    {
        Point[] corners = [
            new(rectangle.X, rectangle.Y),
            new(rectangle.X + rectangle.Width, rectangle.Y),
            new(rectangle.X, rectangle.Y + rectangle.Height),
            new(rectangle.X + rectangle.Width, rectangle.Y + rectangle.Height)];
        return corners.Max(corner =>
            Math.Sqrt((startingPoint.X - corner.X) * (startingPoint.X - corner.X) +
                      (startingPoint.Y - corner.Y) * (startingPoint.Y - corner.Y)));
    }
}