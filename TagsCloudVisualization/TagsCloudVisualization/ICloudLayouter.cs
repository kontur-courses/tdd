using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    internal interface ICloudLayouter
    {
        List<Rectangle> PlacedRectangles { get; }
        Rectangle GetBorders();
        Rectangle PutNextRectangle(Size rectangleSize);
    }
}