using SixLabors.ImageSharp;
using System.Numerics;

namespace TagsCloudVisualization;

public class Layout : ILayout
{
    // Public to enable Unit testing
    public readonly List<RectangleF> PlacedRectangles = new();

    private readonly PointF center;
    private readonly ILayoutFunction layoutFunction;

    public Layout(ILayoutFunction layoutFunction, PointF center)
    {
        this.center = center;
        this.layoutFunction = layoutFunction;
    }

    public RectangleF PutNextRectangle(SizeF rectSize)
    {
        var rectangle = GetCorrectlyPlacedRectangle(rectSize);
        var moved = GetMovedToCenterRectangle(rectangle);

        PlacedRectangles.Add(moved);
        
        return moved;
    }

    private RectangleF GetMovedToCenterRectangle(RectangleF rectangle)
    {
        var currentRect = rectangle;

        // Skip 1-st rectangle, because it's already in (0, 0) point.
        if (PlacedRectangles.Count == 0)
            return currentRect;

        var toCenter = new Vector2(center.X - currentRect.X, center.Y - currentRect.Y);
        var length = (float)Math.Sqrt(toCenter.X * toCenter.X + toCenter.Y * toCenter.Y);
        var normalized = new Vector2(toCenter.X / length, toCenter.Y / length);

        while (true)
        {
            var point = new PointF(currentRect.X + normalized.X, currentRect.Y + normalized.Y);
            var newRect = new RectangleF(point, currentRect.Size);

            if (PlacedRectangles.Any(rect => rect.IntersectsWith(newRect)))
                break;

            currentRect = newRect;
        }

        return currentRect;
    }

    private RectangleF GetCorrectlyPlacedRectangle(SizeF rectSize)
    {
        var point = layoutFunction.GetNextPoint();
        var rectangle = new RectangleF(point, rectSize);

        while (true)
        {
            point = point
                .Center(center)
                .ApplyOffset(-rectSize.Width / 2, -rectSize.Height / 2);

            (rectangle.X, rectangle.Y) = point;

            if (PlacedRectangles.All(rect => !rect.IntersectsWith(rectangle)))
                break;

            point = layoutFunction.GetNextPoint();
        }

        return rectangle;
    }
}