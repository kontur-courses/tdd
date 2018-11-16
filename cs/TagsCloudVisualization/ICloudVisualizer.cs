using System.Collections;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public interface ICloudVisualizer
    {
        Bitmap CreateImage(IEnumerable<Rectangle> rectangles, string path);
    }
}