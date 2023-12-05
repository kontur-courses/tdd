using System.Drawing;

namespace TagsCloudVisualization;

public interface IRectangleDrawer
{
    public Bitmap DrawRectangles(List<Rectangle> rectangles, Rectangle borders);
}