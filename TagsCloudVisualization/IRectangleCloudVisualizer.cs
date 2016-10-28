using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public interface IRectangleCloudVisualizer
    {
        Bitmap GetImageCloud(List<Rectangle> rectangles, Color colorOfRectangle, Color backgroundСolor);
    }
}