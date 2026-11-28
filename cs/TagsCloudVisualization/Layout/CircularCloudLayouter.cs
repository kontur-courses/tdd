using System.Drawing;

namespace TagsCloudVisualization.Layout;

public class CircularCloudLayouter
{
    private readonly ISpiralGen spiral;
    private readonly List<Rectangle> rectangles = new();

    public Point Center { get; }

    public IReadOnlyList<Rectangle> Rectangles => rectangles;

    public CircularCloudLayouter(Point center)
        : this(center, new ArchimedeanSpiralGen(center))
    {
    }

    public CircularCloudLayouter(Point center, ISpiralGen spiral)
    {
        Center = center;
        this.spiral = spiral;
    }

    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        throw new NotImplementedException();
    }
}
