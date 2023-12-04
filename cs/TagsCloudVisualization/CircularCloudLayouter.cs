using System.Drawing;

namespace TagsCloudVisualization;

public class CircularCloudLayouter
{
    private readonly List<Rectangle> rectangles = new();
    private readonly Layout layout;
    private bool isNameSet;

    public Point Center => layout.Center;
    
    public Rectangle[] GetRectangles => rectangles.ToArray();

    public CircularCloudLayouter(Point center)
    {
        layout = new Layout(center);
    }
    
    public Rectangle SetName(Size rectangleSize)
    {
        if (isNameSet)
            throw new InvalidOperationException("Name can be set once");

        isNameSet = true;
        return AddRectangle(rectangleSize);
    }

    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        if (!isNameSet)
            throw new InvalidOperationException("Cannot add tags to unnamed layout");
        
        return AddRectangle(rectangleSize);
    }

    private Rectangle AddRectangle(Size size)
    {
        if (size.Width <= 0 || size.Height <= 0)
            throw new ArgumentException("Width and height of rectangle must be positive");
        
        var leftOffset = size.Width / 2 + (size.Width % 2 == 0 ? 0 : 1);
        var topOffset = size.Height / 2 + (size.Height % 2 == 0 ? 0 : 1);

        foreach (var coord in layout.GetNextCoord())
        {
            var rectangle = new Rectangle(coord.X - leftOffset, coord.Y - topOffset, size.Width, size.Height);
            
            if (!layout.CanPutInCoords(rectangle))
                continue;

            layout.OccupyCoords(rectangle);
            
            return rectangle;
        }

        return default;
    }
}