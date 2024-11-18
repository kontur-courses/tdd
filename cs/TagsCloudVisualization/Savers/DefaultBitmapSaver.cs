using System.Drawing;
using System.Drawing.Imaging;

namespace TagsCloudVisualization.Savers;

public class DefaultBitmapSaver(string bitmapDirectory="images") : IBitmapSaver
{
    public void SaveBitmap(Bitmap bitmap, string filename)
    {
        Directory.CreateDirectory(bitmapDirectory);
        bitmap.Save(Path.Combine(bitmapDirectory, filename), ImageFormat.Jpeg);
    }
}