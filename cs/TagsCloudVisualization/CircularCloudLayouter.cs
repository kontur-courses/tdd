using System.Drawing;
using System.Numerics;
using TagsCloudVisualization.Extensions;

namespace TagsCloudVisualization;

public class CircularCloudLayouter
{
    private readonly Point center;
    private readonly ArchimedeanSpiral spiral;
    public List<Rectangle> Rectangles { get; } = [];
    public CircularCloudLayouter(Point center)
    {
        this.center = center;
        spiral = new ArchimedeanSpiral(center);
    }
    
    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
            throw new ArgumentException("Rectangle size must be positive");

        var rect = PlaceNext(rectangleSize);
        rect = MoveCloserToCenter(rect);
        Rectangles.Add(rect);

        return rect;
    }

    public void Reset()
    {
        Rectangles.Clear();
        spiral.Reset();
    }

    private Rectangle PlaceNext(Size size)
    {
        while (true)
        {
            var point = spiral.GetNextPoint();
            var rect = new Rectangle(point.X - size.Width / 2, point.Y - size.Height / 2, size.Width, size.Height);
            if (!Rectangles.Any(r => r.IntersectsWith(rect)))
                return rect;
        }
    }

    private Rectangle MoveCloserToCenter(Rectangle rectangle)
    {
        var moved = rectangle;
        var centerPosition = new Vector2(center.X, center.Y);
        const float stepSize = 0.5f;

        while (true)
        {
            var rectCenter = rectangle.Center();
            var rectangleCenterPosition = new Vector2(rectCenter.X, rectCenter.Y);
            var direction = centerPosition - rectangleCenterPosition;
            if (direction.LengthSquared() == 0)
                break;

            var unitDirection = Vector2.Normalize(direction);
            var offset = unitDirection * stepSize;
            var testRect = moved with { X = (int)(moved.X + offset.X), Y = (int)(moved.Y + offset.Y) };
            if (IntersectsWithOthers(testRect))
                break;

            moved = testRect;
        }

        return moved;
    }

    private bool IntersectsWithOthers(Rectangle rect)
    {
        return Rectangles.Any(r => r.IntersectsWith(rect));
    }
}