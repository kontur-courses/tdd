using System.Drawing;
using System.Drawing.Imaging;

namespace TagsCloudVisualization;

public class ImageSaver
{
    public static void Save(string filePath, Bitmap bitmap)
    {
        ArgumentNullException.ThrowIfNull(bitmap);
        ArgumentNullException.ThrowIfNull(filePath);

        var directory = Path.GetDirectoryName(filePath);
        if (!string.IsNullOrEmpty(directory))
            Directory.CreateDirectory(directory);

        bitmap.Save(filePath, ImageFormat.Png);
    }
}