using System.Drawing;

namespace TagsCloudVisualization;

public class CircularCloudLayouter
{
    private readonly HashSet<Point> occupiedPixels = new();
    private readonly List<Rectangle> rectangles = new();
    private bool isNameSet;
    
    public Point Center { get; }
    
    public Rectangle[] GetRectangles => rectangles.ToArray();

    public CircularCloudLayouter(Point center)
    {
        Center = center;
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

        foreach (var coord in GetNextCoord())
        {
            var rectangle = new Rectangle(coord.X - leftOffset, coord.Y - topOffset, size.Width, size.Height);
            
            if (!CanPutInCoords(rectangle))
                continue;

            OccupyCoords(rectangle);
            
            return rectangle;
        }

        return default;
    }

    private IEnumerable<Point> GetNextCoord()
    {
        yield return Center + new Size(0, 0);
        var offset = 1;

        while (true)
        {
            for (var dx = -offset; dx <= offset; dx++)
            for (var dy = -offset; dy <= offset; dy++)
            {
                if (!(Math.Abs(dx) != offset || Math.Abs(dy) != offset))
                    continue;

                yield return Center + new Size(dx, dy);
            }

            offset++;
        }
    }

    private bool CanPutInCoords(Rectangle rectangle)
    {
        for (var i = rectangle.Left; i <= rectangle.Right; i++)
        for (var j = rectangle.Top; j <= rectangle.Bottom; j++)
            if (occupiedPixels.Contains(new Point(i, j)))
                return false;

        return true;
    }

    private void OccupyCoords(Rectangle rectangle)
    {
        for (var i = rectangle.Left; i <= rectangle.Right; i++)
        for (var j = rectangle.Top; j <= rectangle.Bottom; j++)
            occupiedPixels.Add(new Point(i, j));
    }
}