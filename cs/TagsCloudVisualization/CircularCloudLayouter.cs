using System.Drawing;

namespace TagsCloudVisualization;

public class CircularCloudLayouter : ICloudLayouter
{
    public List<Rectangle> PlacedRectangles { get; } = new();
    private readonly Point center;

    public CircularCloudLayouter(Point center)
    {
        this.center = center;
    }

    private Rectangle CalculateRectanglePosition(Size rectangleSize, int radiusIncrement, double angleIncrement)
    {
        var rectangle = new Rectangle(Point.Empty, rectangleSize);
        
        var radius = 0d;
        var angle = 0d;
        
        do
        {
            var x = center.X + radius * Math.Cos(angle);
            var y = center.Y + radius * Math.Sin(angle);
            rectangle.Location = new Point((int) x, (int) y);
            
            angle += angleIncrement;
            
            if (!(angle >= 2 * Math.PI))
                continue;
            
            angle = 0;
            radius += radiusIncrement;
        } while (IntersectsWithAny(rectangle));

        return rectangle;
    }
    
    private Rectangle PlaceRectangle(Size rectangleSize)
    {
        var newRectangle = new Rectangle(Point.Empty, rectangleSize);

        if (PlacedRectangles.Count == 0)
        {
            newRectangle.Location = new Point(center.X - newRectangle.Width / 2, center.Y - newRectangle.Height / 2);
            return newRectangle;
        }

        
        const int radiusIncrement = 15;
        const double angleIncrement = 0.2d;

        return CalculateRectanglePosition(rectangleSize, radiusIncrement, angleIncrement);
    }

    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        if (rectangleSize.Height < 0 || rectangleSize.Width < 0)
            throw new ArgumentException("Height and width must be positive");

        var newRectangle = PlaceRectangle(rectangleSize);
        PlacedRectangles.Add(newRectangle);
        return newRectangle;
    }

    private bool IntersectsWithAny(Rectangle newRectangle)
    {
        return PlacedRectangles
            .Any(rectangle => rectangle
                .IntersectsWith(newRectangle));
    }
}