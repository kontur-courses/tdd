using System.Drawing;

namespace TagsCloudVisualization.Layout;

public class RectangleCompactor : IRectangleCompactor
{
    public Rectangle MoveTowardsCenter(Rectangle rectangle, IReadOnlyList<Rectangle> obstacles, Point center)
    {
        var current = rectangle;
        bool moved;

        do
        {
            var previous = current;

            current = TryShift(current, obstacles, Math.Sign(center.X - GetMiddle(current).X), 0);
            current = TryShift(current, obstacles, 0, Math.Sign(center.Y - GetMiddle(current).Y));

            moved = current != previous;
        }
        while (moved);

        return current;
    }

    private static Rectangle TryShift(Rectangle rectangle, IReadOnlyList<Rectangle> obstacles, int dx, int dy)
    {
        var shifted = rectangle with { X = rectangle.X + dx, Y = rectangle.Y + dy };

        return obstacles.Any(shifted.IntersectsWith) ? rectangle : shifted;
    }

    private static Point GetMiddle(Rectangle rectangle)
        => new(rectangle.X + rectangle.Width / 2, rectangle.Y + rectangle.Height / 2);
}