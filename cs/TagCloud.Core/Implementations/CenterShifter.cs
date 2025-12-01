using System.Drawing;
using TagsCloudLib.Abstractions;

namespace TagsCloudLib.Implementations;

public class CenterShifter : ICenterShifter
{
    public Rectangle ShiftToCenter(Rectangle rectangle, Point center, IEnumerable<Rectangle> others)
    {
        var dx = Math.Sign(center.X - (rectangle.X + rectangle.Width / 2));
        var dy = Math.Sign(center.Y - (rectangle.Y + rectangle.Height / 2));
        if (dx == 0 && dy == 0)
        {
            return rectangle;
        }

        while (true)
        {
            var moved = new Rectangle(rectangle.X + dx, rectangle.Y + dy, rectangle.Width, rectangle.Height);
            if (others.Any(r => r.IntersectsWith(moved)) || GetDistance(moved, center) > GetDistance(rectangle, center))
            {
                return rectangle;
            }

            rectangle = moved;
        }
    }

    private double GetDistance(Rectangle rectangle, Point center)
    {
        var cx = rectangle.X + rectangle.Width / 2;
        var cy = rectangle.Y + rectangle.Height / 2;
        return Math.Sqrt((cx - center.X) * (cx - center.X) + (cy - center.Y) * (cy - center.Y));
    }
}