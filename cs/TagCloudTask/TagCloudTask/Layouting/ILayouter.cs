using System.Collections.Generic;
using System.Drawing;

namespace TagCloudTask.Layouting
{
    public interface ILayouter
    {
        List<Rectangle> GetRectanglesCopy();

        Size GetRectanglesBoundaryBox();

        Rectangle PutNextRectangle(Size rectangleSize);
    }
}