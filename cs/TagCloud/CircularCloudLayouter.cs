using System.Drawing;

namespace TagCloud;

public class CircularCloudLayouter
{
    private List<Rectangle> rectangles;
    private Point center;
    private ICloudShaper shaper;
    
    public IEnumerable<Rectangle> Rectangles => rectangles;
    
    public CircularCloudLayouter(Point center)
    {
        this.center = center;
        rectangles = new List<Rectangle>();
        shaper = new SpiralCloudShaper(this.center);
    }

    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        var rectangle = shaper.GetNextPossibleRectangle(rectangleSize);
        while (Rectangles.Any(rect => rect.IntersectsWith(rectangle)))
        {
            rectangle = shaper.GetNextPossibleRectangle(rectangleSize);
        }
        rectangles.Add(rectangle);
        return rectangle;
    }
}