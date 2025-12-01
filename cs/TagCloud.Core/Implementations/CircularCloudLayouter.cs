using System.Drawing;
using TagsCloudLib.Abstractions;
using TagsCloudLib.Extensions;

namespace TagsCloudLib.Implementations;

public class CircularCloudLayouter(Point center, ISpiral spiral, ICenterShifter centerShifter, int maxAttempts = 1000)
    : CircularCloudLayouterBase(center)
{
    public override Rectangle PutNextRectangle(Size size)
    {
        if (size.Width <= 0 || size.Height <= 0)
        {
            throw new ArgumentException("Rectangle size must be greater than zero");
        }

        var rectangle = FindFreeRectangle(size);
        rectangle = centerShifter.ShiftToCenter(rectangle, Center, Rectangles);

        AddRectangle(rectangle);
        return rectangle;
    }

    private Rectangle FindFreeRectangle(Size size)
    {
        for (var i = 0; i < maxAttempts; i++)
        {
            var point = spiral.GetNextPoint();
            var candidate = CreateRectangleCenteredAt(point, size);

            if (!candidate.Intersects(Rectangles))
            {
                return candidate;
            }
        }

        throw new InvalidOperationException(
            $"Failed to place rectangle {size.Width}x{size.Height} after {maxAttempts} attempts");
    }

    private static Rectangle CreateRectangleCenteredAt(Point center, Size size)
    {
        return new Rectangle(center.X - size.Width / 2, center.Y - size.Height / 2, size.Width, size.Height);
    }
}