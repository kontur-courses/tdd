using System.Drawing;
using System.Drawing.Imaging;

#pragma warning disable CA1416
namespace TagsCloudVisualization.Visualization;

public class ImageSaver
{
    public static void Save(Bitmap bitmap, string filePath, string fileName, ImageFormat imageFormat)
    {
        bitmap.Save(Path.Combine(filePath, fileName), imageFormat);
    }
}