using System.Collections.Generic;
using System.Drawing;

namespace TagCloud.Layouting
{
    public interface ICloudLayouter
    {
        Point Center { get; }
        Rectangle PutNextRectangle(Size rectangleSize);

        Size GetRectanglesBoundaryBox();

        List<Rectangle> GetRectanglesCopy();

        int GetCloudBoundaryRadius();
    }
}