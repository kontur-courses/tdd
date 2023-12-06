using System.Drawing;

namespace TagsCloudVisualization;

public class Cloud
{
    private Point Center { get; set; }
    public List<Rectangle> Rectangles { get; private set; }

    public Cloud(Point center, List<Rectangle> rectangles)
    {
        Center = center;
        Rectangles = rectangles ?? new List<Rectangle>();
    }

    public void AddRectangle(Rectangle rectangle)
    {
        Rectangles.Add(rectangle);
    }
}