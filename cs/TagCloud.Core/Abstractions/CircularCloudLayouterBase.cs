using System.Drawing;

namespace TagsCloudLib.Abstractions;

public abstract class CircularCloudLayouterBase(Point center)
{
    public Point Center { get; } = center;
    private readonly List<Rectangle> rectangles = [];
    public IReadOnlyList<Rectangle> Rectangles => rectangles;
    
    protected void AddRectangle(Rectangle rectangle)
    {
        rectangles.Add(rectangle);
    }
    public abstract Rectangle PutNextRectangle(Size rectangleSize);
}