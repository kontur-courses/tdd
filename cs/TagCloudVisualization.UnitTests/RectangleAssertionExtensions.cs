using System.Drawing;

namespace TagCloudVisualization.UnitTests;

public static class RectangleAssertionExtensions
{
    public static bool TouchesWith(this Rectangle rectangle, Rectangle other)
    {
        return !rectangle.IntersectsWith(other) && (
            rectangle.Left == other.Right && BoundsTopBottom(rectangle, other)
            || rectangle.Right == other.Left && BoundsTopBottom(rectangle, other)
            || rectangle.Top == other.Bottom && BoundsLeftRight(rectangle, other)
            || rectangle.Bottom == other.Top && BoundsLeftRight(rectangle, other));

        static bool BoundsTopBottom(Rectangle rectangle, Rectangle other)
        {
            return (rectangle.Top <= other.Bottom && rectangle.Bottom >= other.Top);
        }

        static bool BoundsLeftRight(Rectangle rectangle, Rectangle other)
        {
            return (rectangle.Left <= other.Right && rectangle.Right >= other.Left);
        }
    }
}