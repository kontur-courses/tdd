using System.Drawing;

namespace TagsCloudVisualization;

public abstract class BaseCloudLayouter : ITagsCloudLayouter
{
    protected Point center { get; }
    private readonly List<Rectangle> rectangles;
    public IEnumerable<Rectangle> Rectangles => rectangles;

    protected BaseCloudLayouter(Point center)
    {
        this.center = center;
        rectangles = new List<Rectangle>();
    }
    
    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        var rectPosition = FindPositionForRectangle(rectangleSize);
        
        var rectangle = new Rectangle(rectPosition, rectangleSize);
        rectangles.Add(rectangle);
        return rectangle;
    }

    public abstract Point FindPositionForRectangle(Size rectangleSize);

    public bool IsPlaceSuitableForRectangle(Rectangle rectangle)
    {
        return rectangles.All(x => !x.IntersectsWith(rectangle));
    }
}