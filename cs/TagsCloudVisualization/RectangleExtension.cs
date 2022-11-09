namespace TagsCloudVisualization;

public static class RectangleExtension
{
    public static Point GetCenter(this Rectangle rectangle)
    { 
        var center = new Point((rectangle.X + rectangle.Right) / 2, (rectangle.Y + rectangle.Bottom) / 2);
        return center;
    }
}