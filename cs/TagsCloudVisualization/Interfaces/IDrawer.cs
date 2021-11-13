using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization.Interfaces
{
    public interface IDrawer
    { 
        Bitmap DrawRectangles(IEnumerable<Rectangle> rectangles, Size imageSize);
    }
}