using System.Collections.Generic;
using System.Drawing;

namespace TagCloud
{
    public interface IRectanglePlacer
    {
        Rectangle PlaceRectangle(Size size, Point startPoint, IReadOnlyCollection<Rectangle> reservedSpace);
    }
}