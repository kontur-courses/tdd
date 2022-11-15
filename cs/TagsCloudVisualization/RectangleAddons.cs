using System.Drawing;

namespace TagsCloudVisualization
{
    public class RectangleAddons
    {
        public static bool IsRectanglesIntersect(Rectangle firstRect, Rectangle secondRect)
        {
            return !Rectangle.Intersect(firstRect, secondRect).IsEmpty;
        }
    }
}