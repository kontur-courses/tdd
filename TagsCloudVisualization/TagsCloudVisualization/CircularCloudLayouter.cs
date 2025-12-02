using System.Drawing;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("TagsCloudVisualizationTests")]
namespace TagsCloudVisualization;

public class CircularCloudLayouter : ILayouter
{
    private readonly List<Rectangle> _prevRectangles;
    private readonly IPointGenerator _pointGenerator;
    private const int Density = 1;
    private Point _center;
    public CircularCloudLayouter(Point center)
    {
        _pointGenerator = new ArchimedeanSpiralPointGenerator(center, Density);
        _prevRectangles = new List<Rectangle>();
        this._center = center;
    }

    public IEnumerable<Rectangle> GetLayout()
    {
        foreach (var rectangle in _prevRectangles)
            yield return rectangle;
    }

    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        foreach (var nextRectangleCenter in _pointGenerator.GetNextPoint())
        {
            var nextRectanglePos = nextRectangleCenter - rectangleSize / 2;
            if (CheckNextRectIntersection(nextRectanglePos, rectangleSize, out var nextRect))
                continue;
            var pressedNextRect = new Rectangle(PressRectangleToCenter(nextRect), rectangleSize);
            _prevRectangles.Add(pressedNextRect);
            return pressedNextRect;
        }
        throw new Exception("No more space available");
    }

    internal Point PressRectangleToCenter(Rectangle rectangle)
    {
        var rectPos = rectangle.Location;
        while (true)
        {
            var rectCenter = new Point(
                rectPos.X + rectangle.Width / 2,
                rectPos.Y + rectangle.Height / 2);
            var direction = _center.Subtract(rectCenter);
            if (direction == Point.Empty)
                return rectPos;
            var nextPos = rectPos.Add(direction);
            if (CheckNextRectIntersection(nextPos, rectangle.Size, out _))
                return rectPos;
            rectPos = nextPos;
        }
    }

    private bool CheckNextRectIntersection(Point nextPos, Size size, out Rectangle nextRect)
    {
        var possibleNextRect = new Rectangle(nextPos, size);
        if (_prevRectangles.Any(x => x.IntersectsWith(possibleNextRect)))
        {
            nextRect = Rectangle.Empty;
            return true;
        }
        nextRect = possibleNextRect;
        return false;
    }
}