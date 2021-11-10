using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public interface ICloudLayouter
    {
        IReadOnlyList<Rectangle> Rectangles { get; } 
        Rectangle PutNextRectangle(Size rectangleSize);
    }
}