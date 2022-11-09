namespace TagsCloudVisualization;

public static class RectangleExtension
{
    public static Point GetCenter(this Rectangle rectangle)
    { 
        var center = new Point((rectangle.X + rectangle.Right) / 2, (rectangle.Y + rectangle.Bottom) / 2);
        return center;
    }
    public static double GetDiagonal(this Size rect)
    {
        return Math.Sqrt(rect.Width * rect.Width + rect.Height * rect.Height);
    }
    public static double GetDiagonal(this Rectangle rect)
    {
        return Math.Sqrt(rect.Width * rect.Width + rect.Height * rect.Height);
    }
}