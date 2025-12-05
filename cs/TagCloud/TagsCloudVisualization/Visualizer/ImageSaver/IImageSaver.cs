using System.Drawing;
using System.Drawing.Imaging;

namespace TagsCloudVisualization;

public interface IImageSaver
{
    string SaveBitmap(Bitmap bitmap, string fileName, ImageFormat format = null);
}