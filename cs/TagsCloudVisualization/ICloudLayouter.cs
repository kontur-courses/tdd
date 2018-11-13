using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public interface ICloudLayouter
    {
        Point Center { get; }
        List<Rectangle> Rectangles { get; }
        int Width { get; }
        int Height { get; }
        Rectangle PutNextRectangle(Size rectangleSize);
    }
}