using System.Drawing;

namespace TagsCloudVisualization;

public class CircularCloudBuilder : ICircularCloudBuilder
{
    public Point Center { get; }
    public int RadiusIncrement { get; }
    public double AngleIncrement { get; }

    public CircularCloudBuilder(Point center, int radiusIncrement, double angleIncrement)
    {
        Center = center;
        RadiusIncrement = radiusIncrement;
        AngleIncrement = angleIncrement;
    }

    private static void ValidateArguments(Size rectangleSize, List<Rectangle> placedRectangles)
    {
        if (placedRectangles is null)
            throw new ArgumentException("The list of placed rectangles cannot be null.");
        if (rectangleSize.Height < 0 || rectangleSize.Width < 0)
            throw new ArgumentException("Height and width must be positive");
    }

    private Rectangle PlaceFirstRectangle(Size rectangleSize)
    {
        var firstRectangleLocation = new Point(Center.X - rectangleSize.Width / 2,
            Center.Y - rectangleSize.Height / 2);
        return new Rectangle(firstRectangleLocation, rectangleSize);
    }
    
    public Rectangle GetNextPosition(Size rectangleSize, List<Rectangle> placedRectangles)
    {
        ValidateArguments(rectangleSize, placedRectangles);

        if (placedRectangles.Count == 0)
            return PlaceFirstRectangle(rectangleSize);
        
        var rectangle = new Rectangle(Point.Empty, rectangleSize);
        var radius = 0d;
        var angle = 0d;
        
        do
        {
            var x = Center.X + radius * Math.Cos(angle);
            var y = Center.Y + radius * Math.Sin(angle);
            rectangle.Location = new Point((int) x, (int) y);
            
            angle += AngleIncrement;
            
            if (!(angle >= 2 * Math.PI))
                continue;
            
            angle = 0;
            radius += RadiusIncrement;
        } while (IntersectsWithAny(rectangle, placedRectangles));
        
        return rectangle;
    }
    
    private static bool IntersectsWithAny(Rectangle newRectangle, IEnumerable<Rectangle> placedRectangles)
    {
        return placedRectangles
            .Any(rectangle => rectangle
                .IntersectsWith(newRectangle));
    }
}