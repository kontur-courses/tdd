using System.Collections.Generic;
using System.Drawing;

namespace TagCloudTask.Layouting
{
    public interface ILayouter
    {
        Size GetRectanglesBoundaryBox();

        List<Rectangle> GetRectanglesCopy();

        Rectangle PutNextRectangle(Size rectangleSize);
    }
}