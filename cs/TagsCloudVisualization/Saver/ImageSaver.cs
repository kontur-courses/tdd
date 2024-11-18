using System.Drawing.Imaging;
using TagsCloudVisualization.Draw;

namespace TagsCloudVisualization.Saver;

public static class ImageSaver
{
    public static void SaveImageToFile(this RectangleDraftsman rectangleDraftsman, string filename)
    {
        #pragma warning disable CA1416
        rectangleDraftsman.Bitmap.Save(filename, ImageFormat.Png);
        #pragma warning restore CA1416
        Console.WriteLine($"Tag cloud visualization saved to: {Path.GetFullPath(filename)}");
    }
}