using System.Drawing;

namespace TagsCloudVisualization.Base;

public class RectanglesWindow
{
    public int Width => end.X - position.X;
    public int Height => end.Y - position.Y;
    public Point Center => new(position.X + Width / 2, position.Y + Height / 2);
    private Point end = new(int.MinValue, int.MinValue);
    private Point position = new(int.MaxValue, int.MaxValue);
    
    public RectanglesWindow(IEnumerable<Rectangle> rectangles)
    {
        PutRectangles(rectangles);
    }

    private void PutRectangles(IEnumerable<Rectangle> rectangles)
    {
        foreach (var rectangle in rectangles)
            PutRectangle(rectangle);
    }

    private void PutRectangle(Rectangle rectangle)
    {
        position = new Point
        {
            X = Math.Min(rectangle.Left, position.X),
            Y = Math.Min(rectangle.Top, position.Y)
        };
        
        end.X = Math.Max(rectangle.Right, end.X);
        end.Y = Math.Max(rectangle.Bottom, end.Y);
    }
}