using System.Drawing;

namespace TagsCloudVisualization.Utils;

public static class Utils
{
    public static int CalculateShortestDistance(Rectangle rect1, Rectangle rect2)
    {
        var horizontalDistance = int.MaxValue;
        var verticalDistance = int.MaxValue;

        if (rect1.X + rect1.Width <= rect2.X)
            horizontalDistance = rect2.X - (rect1.X + rect1.Width);
        else if (rect2.X + rect2.Width <= rect1.X)
            horizontalDistance = rect1.X - (rect2.X + rect2.Width);

        if (rect1.Y + rect1.Height <= rect2.Y)
            verticalDistance = rect2.Y - (rect1.Y + rect1.Height);
        else if (rect2.Y + rect2.Height <= rect1.Y)
            verticalDistance = rect1.Y - (rect2.Y + rect2.Height);

        return Math.Min(horizontalDistance, verticalDistance);
    }

    public static List<Size> GenerateSizes(
        int amount,
        int minWidth = 10,
        int maxWidth = 100,
        int minHeight = 10,
        int maxHeight = 100)
    {
        var rnd = new Random();
        var sizes = new List<Size>(amount);
        for (var i = 0; i < amount; i++)
            sizes.Add(new Size(rnd.Next(minWidth, maxWidth), rnd.Next(minHeight, maxHeight)));

        return sizes;
    }

    public static Rectangle GetRectangleFromCenter(Point center, Size size)
    {
        var x = center.X - size.Width / 2;
        var y = center.Y - size.Height / 2;

        return new Rectangle(new Point(x, y), size);
    }

    public static Point GetRectangleCenter(Rectangle rectangle)
    {
        var x = rectangle.X;
        var y = rectangle.Y;
        return new Point(x + rectangle.Width / 2, y + rectangle.Height / 2);
    }
}