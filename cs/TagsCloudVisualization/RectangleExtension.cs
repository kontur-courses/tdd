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

    public static int GetSquare(this Rectangle rectangle)
    {
        return rectangle.Width * rectangle.Height;
    }

    public static void MoveTo(this Rectangle rectangle, int x, int y)
    {
        rectangle.X = x;
        rectangle.Y = y;
    }
}