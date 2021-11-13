using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization.Drawing
{
    public interface IDrawer
    {
        Bitmap DrawRectangles(IEnumerable<Rectangle> rectangles, Size imageSize);
    }
}