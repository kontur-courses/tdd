using System.Drawing;
using System;

namespace TagsCloudVisualization;
public static class Utils
{
    public static int CalculateShortestDistance(Rectangle rect1, Rectangle rect2)
    {
        var horizontalDistance = Int32.MaxValue;
        var verticalDistance = Int32.MaxValue;

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
}
