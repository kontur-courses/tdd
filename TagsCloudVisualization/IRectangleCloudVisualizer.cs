using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public interface IRectangleCloudVisualizer
    {
        Bitmap GetImageCloud(List<Rectangle> rectangles, int width, int height, Color colorOfRectangle, Color backgroundСolor);
    }
}