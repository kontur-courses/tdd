using System.Drawing;

namespace TagsCloudVisualization;

public class Cloud
{
    public Cloud(Point center, List<Rectangle> rectangles)
    {
        Center = center;
        Rectangles = rectangles ?? new List<Rectangle>();
    }

    public Point Center { get; private set; }
    public List<Rectangle> Rectangles { get; }

    public void AddRectangle(Rectangle rectangle)
    {
        Rectangles.Add(rectangle);
    }

    public int GetWidth()
    {
        return Rectangles.Max(rect => rect.X) - Rectangles.Min(rect => rect.X);
    }

    public int GetHeight()
    {
        return Rectangles.Max(rect => rect.Y) - Rectangles.Min(rect => rect.Y);
    }
}