using System.Drawing;

namespace TagsCloudVisualization;

public interface ICloudRenderer
{
    public Bitmap CreateRectangleCloud(IEnumerable<Rectangle> rectangles);
}