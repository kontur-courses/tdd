using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
    internal interface ICloudLayout
    {
        List<Rectangle> PlacedRectangles { get; }
        Rectangle GetBorders();
        Rectangle PutNextRectangle(Size rectangleSize);
    }
}
