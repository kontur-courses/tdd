using System.Drawing;

namespace TagCloudVisualization.Layout;

public class RectangleCollisionIndex
{
    private readonly List<Rectangle> rectangles = new();
    public IReadOnlyList<Rectangle> Rectangles => rectangles;

    public bool IsFree(Rectangle candidate) =>
        !rectangles.Any(r => r.IntersectsWith(candidate));

    public void Add(Rectangle rectangle) => rectangles.Add(rectangle);
}