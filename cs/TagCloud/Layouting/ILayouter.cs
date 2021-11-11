using System.Collections.Generic;
using System.Drawing;

namespace TagCloud.Layouting
{
    public interface ILayouter
    {
        Size GetRectanglesBoundaryBox();

        List<Rectangle> GetRectanglesCopy();

        Rectangle PutNextRectangle(Size rectangleSize);
    }
}