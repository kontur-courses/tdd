using System.Drawing;

namespace TagsCloudVisualization;

public class CircularCloudLayouter
{
    private readonly Point center;
    private readonly List<Rectangle> rectangles;
    private readonly IPointGenerator pointGenerator;
    
    public CircularCloudLayouter(Point center) : this(center, new ArchimedeanSpiral()) { }

    public CircularCloudLayouter(Point center, IPointGenerator pointGenerator)
    {
        this.center = center;
        this.pointGenerator = pointGenerator;
        rectangles = new();
    }

    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        throw new NotImplementedException();
    }
}
