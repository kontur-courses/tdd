namespace TagsCloudVisualization.Visualizers;
using System.Drawing;

public interface IVisualizer
{
    public Bitmap CreateBitmap(IEnumerable<Rectangle> rectangles);
}