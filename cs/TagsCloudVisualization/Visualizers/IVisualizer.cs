using System.Drawing;

namespace TagsCloudVisualization.Visualizers;

public interface IVisualizer
{
    public Bitmap CreateBitmap(IEnumerable<Rectangle> rectangles, Size bitmapSize);
}