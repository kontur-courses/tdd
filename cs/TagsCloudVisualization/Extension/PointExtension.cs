using System.Drawing;

namespace TagsCloudVisualization.Extension;

public static class PointExtension
{
    public static Point Add(this Point selfPoint, Point otherPoint) =>
        new(selfPoint.X + otherPoint.X, selfPoint.Y + otherPoint.Y);

    public static Point Subtract(this Point selfPoint, Point otherPoint) =>
        new(selfPoint.X - otherPoint.X, selfPoint.Y - otherPoint.Y);
}