using System.Drawing;

namespace TagsCloud;

public interface IRectanglesVisualizer
{
    Bitmap GetTagsCloudImage(List<Rectangle> rectangles);
}