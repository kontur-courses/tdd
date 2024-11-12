using System.Drawing;
using System.Drawing.Imaging;

namespace TagsCloudVisualization;

public interface ITagsCloudVisualizer : IDisposable
{
    public void AddVisualizationRectangles(IEnumerable<Rectangle> rectangles);
    public void Save(string outputFilePath, ImageFormat imageFormat);
}