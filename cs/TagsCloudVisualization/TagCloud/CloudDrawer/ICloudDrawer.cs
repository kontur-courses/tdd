using System.Drawing;
using System.Drawing.Imaging;

namespace TagsCloudVisualization.TagCloud;

public interface ICloudDrawer
{
    public Bitmap DrawCloud(List<Rectangle> rectangles,  int imageWidth, int imageHeight);
    public void SaveToFile(Bitmap bitmap, string? fileName, string? path, ImageFormat format);
}