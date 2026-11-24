using System.Drawing;
using TagCloudVisualization.Geometry;
using TagCloudVisualization.Layout;

namespace TagCloudVisualization;

public class CircularCloudLayouter
{
    public Point Center { get; }
    private readonly ArchimedeanSpiral spiral;
    private readonly RectangleCollisionIndex index = new();
    private readonly CloudCompactor compactor;

    public IReadOnlyList<Rectangle> Rectangles => index.Rectangles; 

    public CircularCloudLayouter(Point center)
    {
        Center = center;
        spiral = new ArchimedeanSpiral(center);
        compactor = new CloudCompactor(center, index);
    }

    public Rectangle PutNextRectangle(Size size)
    {
        if (size.Width < 0 || size.Height < 0)
            throw new ArgumentException("Size must be non-negative", nameof(size));

        var rect = FindPlace(size);
        index.Add(rect);
        return rect;
    }

    private Rectangle FindPlace(Size size)
    {
        foreach (var p in spiral.GetPoints())
        {
            var candidate = new Rectangle(
                new Point(p.X - size.Width / 2, p.Y - size.Height / 2), size);
            if (index.IsFree(candidate))
                return compactor.MoveTowardsCenter(candidate);
        }
        throw new InvalidOperationException(); 
    }
}