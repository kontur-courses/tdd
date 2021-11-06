using System.Collections.Generic;
using System.Drawing;

namespace TagCloud.Layouting
{
    public interface ICloudLayouter
    {
        Rectangle PutNextRectangle(Size rectangleSize);

        Size GetRectanglesBoundaryBox();

        List<Rectangle> GetRectangles();

        Point GetCloudCenter();

        int GetCloudRadius();

    }
}
