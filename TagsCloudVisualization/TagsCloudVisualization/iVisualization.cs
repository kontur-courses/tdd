using System.Collections.Generic;
using System.Drawing;


namespace TagsCloudVisualization
{
    internal interface IVisualization
    {   
        Bitmap DrawRectangles(List<Rectangle> rectangles);
    }
}
