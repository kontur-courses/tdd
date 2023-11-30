using System.Drawing;

namespace TagsCloudVisualization;

public class CircularCloudLayouter : ICloudLayouter
{
    private readonly Point center;
    public List<Rectangle> Rectangles { get; }

    public CircularCloudLayouter(Point center)
    {
        this.center = center;
        Rectangles = new List<Rectangle>();
    }

    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        if (rectangleSize.Height < 0 || rectangleSize.Width < 0)
            throw new ArgumentException("Width and height must be positive.");

        if (Rectangles.Count == 0)
        {
            Rectangles.Add(new Rectangle(
                center.X - rectangleSize.Width / 2,
                center.Y - rectangleSize.Height / 2,
                rectangleSize.Width,
                rectangleSize.Height));
            return Rectangles[0];
        }
        
        var newRectangle = GetRectanglePosition(rectangleSize);
        return newRectangle;
    }

    public Rectangle GetRectanglePosition(Size rectangleSize)
    {
        return new Rectangle();
    }
}