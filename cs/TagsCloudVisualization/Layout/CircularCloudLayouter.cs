using System.Drawing;

namespace TagsCloudVisualization.Layout;

public class CircularCloudLayouter
{
    private readonly List<Rectangle> rectangles = new();
    private readonly ISpiralGen spiral;

    public CircularCloudLayouter(Point center)
        : this(new ArchimedeanSpiralGen(center))
    {
    }

    public CircularCloudLayouter(ISpiralGen spiral)
    {
        this.spiral = spiral;
        Center = spiral.Center;
    }

    public Point Center { get; }

    public IReadOnlyList<Rectangle> Rectangles => rectangles;

    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(rectangleSize),
                "Rectangle width and height must be positive.");
        }

        var candidate = FindFirstFreeRectangleOnSpiral(rectangleSize);
        var tightened = ShiftTowardsCenter(candidate);
        rectangles.Add(tightened);
        return tightened;
    }

    private Rectangle FindFirstFreeRectangleOnSpiral(Size size)
    {
        foreach (var point in spiral.GetPoints())
        {
            var candidate = RectangleExtensions.FromCenter(point, size);
            if (!candidate.IntersectsWithAny(rectangles))
            {
                return candidate;
            }
        }

        throw new InvalidOperationException(
            "Spiral generator stopped producing points before a free position was found.");
    }

    private Rectangle ShiftTowardsCenter(Rectangle rectangle)
    {
        var target = RectangleExtensions.FromCenter(Center, rectangle.Size).Location;

        while (true)
        {
            var dx = target.X - rectangle.X;
            var dy = target.Y - rectangle.Y;

            var moved = Math.Abs(dx) >= Math.Abs(dy)
                ? TryStepX(ref rectangle, Math.Sign(dx)) | TryStepY(ref rectangle, Math.Sign(dy))
                : TryStepY(ref rectangle, Math.Sign(dy)) | TryStepX(ref rectangle, Math.Sign(dx));

            if (!moved)
            {
                return rectangle;
            }
        }
    }

    private bool TryStepX(ref Rectangle rectangle, int stepX)
    {
        if (stepX == 0)
        {
            return false;
        }

        var shifted = new Rectangle(rectangle.X + stepX, rectangle.Y, rectangle.Width, rectangle.Height);
        if (shifted.IntersectsWithAny(rectangles))
        {
            return false;
        }

        rectangle = shifted;
        return true;
    }

    private bool TryStepY(ref Rectangle rectangle, int stepY)
    {
        if (stepY == 0)
        {
            return false;
        }

        var shifted = new Rectangle(rectangle.X, rectangle.Y + stepY, rectangle.Width, rectangle.Height);
        if (shifted.IntersectsWithAny(rectangles))
        {
            return false;
        }

        rectangle = shifted;
        return true;
    }
}