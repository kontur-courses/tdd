using System.Drawing;

namespace TagsCloudVisualization;

public static class RectangleExtensions
{
    public static Point GetCenter(this Rectangle rect) => 
        new Point(rect.X + rect.Width / 2, rect.Y + rect.Height / 2);
}