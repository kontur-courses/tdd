using System.Drawing;
using System.Drawing.Imaging;

namespace TagsCloudVisualization;

public class ImageSaver : IImageSaver
{
    public void Save(Bitmap bitmap, string fileName)
    {   
        var projectDir = GetProjectDirectory();
        var imagesDir = Path.Combine(projectDir, "Image");
        Directory.CreateDirectory(imagesDir);
        var path = Path.Combine(imagesDir, fileName);
        bitmap.Save(path, ImageFormat.Png);
    }

    private static string GetProjectDirectory()
    {
        var dir = new DirectoryInfo(Environment.CurrentDirectory);
        while (dir != null && !dir.GetFiles("*.csproj").Any())
        {
            dir = dir.Parent;
        }
        return dir?.FullName ?? Environment.CurrentDirectory;
    }
}