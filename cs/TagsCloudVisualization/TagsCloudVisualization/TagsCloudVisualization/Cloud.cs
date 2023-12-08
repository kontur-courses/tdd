using System.Drawing;

namespace TagsCloudVisualization;

public class Cloud
{
    public Point Center { get; private set; }
    public List<Rectangle> Rectangles { get; private set; }

    public Cloud(Point center, List<Rectangle> rectangles)
    {
        Center = center;
        Rectangles = rectangles ?? new List<Rectangle>();
    }

    public void AddRectangle(Rectangle rectangle) =>
        Rectangles.Add(rectangle);

    public int GetWidth() =>
        Rectangles.Max(rect => rect.X) - Rectangles.Min(rect => rect.X);

    public int GetHeight() =>
        Rectangles.Max(rect => rect.Y) - Rectangles.Min(rect => rect.Y);
}