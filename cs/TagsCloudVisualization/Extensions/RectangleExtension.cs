using System.Drawing;

namespace TagsCloudVisualization.Extensions;

public static class RectangleExtension
{
    public static Point GetCentralPoint(this Rectangle rectangle)
    {
        var centerPoint = rectangle.Location;
        centerPoint.Offset(rectangle.Width / 2, -rectangle.Height / 2);
        return centerPoint;
    }
}

