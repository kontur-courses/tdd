using System.Drawing;

namespace TagCloud;

public class CircularCloudLayouter
{
    private List<Rectangle> rectangles;
    private Point center;
    
    public IEnumerable<Rectangle> Rectangles => rectangles;
    
    public CircularCloudLayouter(Point center)
    {
        this.center = center;
        rectangles = new List<Rectangle>();
    }

    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        var rectangle = new Rectangle(center, rectangleSize);
        rectangles.Add(rectangle);
        return rectangle;
    }
}