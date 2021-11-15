using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public interface IRectanglesCloud
    {
        IReadOnlyList<RectangleF> Rectangles { get; }
        PointF Center { get; }
    }
}
