using System.Drawing;
using System.Drawing.Imaging;

namespace TagsCloudVisualization;

public class BitmapSaver(string saveDirectory)
{
    private string saveDirectory = saveDirectory;

    public string SaveBitmap(Bitmap bitmap, string fileName)
    {
        Directory.CreateDirectory(saveDirectory);
        var filepath = Path.Combine(saveDirectory, fileName);
        bitmap.Save(filepath, ImageFormat.Jpeg);
        
        return Path.Combine(Directory.GetCurrentDirectory(), filepath);
    }
}