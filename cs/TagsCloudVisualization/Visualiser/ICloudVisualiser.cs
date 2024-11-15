using System.Drawing;

namespace TagsCloudVisualization;

public interface ICloudVisualiser
{
    public void VisualiseAndSave(IEnumerable<Rectangle> rectangles, string path);
}
