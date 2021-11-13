using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public interface IDrawer
    { 
        Bitmap DrawRectangles(IEnumerable<Rectangle> rectangles, Size imageSize);
    }
}