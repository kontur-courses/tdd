namespace TagsCloudVisualization;

public static class PointExtension
{
    public static Rectangle GetRectangle(this Point center, int x, int y)
    {
        int left = center.X - (x / 2);
        int right = center.Y - (y / 2);
        Point angle = new(left, right);
        return new Rectangle(angle, new Size(x, y));
    }
}