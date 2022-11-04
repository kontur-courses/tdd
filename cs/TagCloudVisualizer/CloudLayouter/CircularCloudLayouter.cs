using System.Drawing;

namespace TagCloudVisualizer.CloudLayouter;

public class CircularCloudLayouter : ICloudLayouter
{
    private readonly Point center;
    private readonly FermatSpiral fermatSpiral;
    private readonly List<Rectangle> rectangles = new ();
    public IReadOnlyList<Rectangle> Rectangles => rectangles;

    public CircularCloudLayouter(Point center)
    {
        this.center = center;
        fermatSpiral = new FermatSpiral(center);
    }
    
    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        if (rectangleSize.Width < 0 || rectangleSize.Height < 0)
            throw new ArgumentException($"Rectangle size should not be negative. Received Width: {rectangleSize.Width} and Height: {rectangleSize.Height}");

        Rectangle result;

        do
        {
            var rectangleCenter = fermatSpiral.GetNextPoint();
            var anchorPoint = GetAnchorPoint(rectangleCenter, rectangleSize);
            result = new Rectangle(anchorPoint, rectangleSize);
        } while (rectangles.Any(r => r.IntersectsWith(result)));
        
        rectangles.Add(result);

        return result;
    }

    private Point GetAnchorPoint(Point rectangleCenter, Size rectangleSize)
    {
        return new Point(rectangleCenter.X - rectangleSize.Width / 2, rectangleCenter.Y - rectangleSize.Height / 2);
    }
}