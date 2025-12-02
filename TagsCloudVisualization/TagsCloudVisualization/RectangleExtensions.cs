using System.Drawing;

namespace TagsCloudVisualization;

public static class RectangleExtensions
{
    public static Point GetCenter(this Rectangle rectangle) =>
        rectangle.Location + rectangle.Size / 2;
}