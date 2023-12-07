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

    public Rectangle PutNextRectangle(Size size)
    {
        if (size.Width <= 0)
            throw new ArgumentException("Size width must be positive number");
        if (size.Height <= 0)
            throw new ArgumentException("Size height must be positive number");
        
        var point = shaper.GetNextPossiblePoint();
        var rectangle = new Rectangle(point, size);
        while (Rectangles.Any(rect => rect.IntersectsWith(rectangle)))
        {
            point = shaper.GetNextPossiblePoint();
            rectangle = new Rectangle(point, size);
        }
        rectangles.Add(rectangle);
        return rectangle;
    }
}