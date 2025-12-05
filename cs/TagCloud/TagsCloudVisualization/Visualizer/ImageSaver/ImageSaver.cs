using System.Drawing;
using System.Drawing.Imaging;

namespace TagsCloudVisualization;

public class ImageSaver : IImageSaver
{
    private readonly string _relativeOutputDirectory;

    public ImageSaver(string relativeOutputDirectory = "out")
    {
        _relativeOutputDirectory = relativeOutputDirectory;
    }

    public string SaveBitmap(Bitmap bitmap, string fileName, ImageFormat format = null)
    {
        ArgumentNullException.ThrowIfNull(bitmap);

        if (string.IsNullOrWhiteSpace(fileName))
            throw new ArgumentException("File name cannot be empty", nameof(fileName));
        
        var projectDir = GetProjectDirectory();
        var outputDir = Path.Combine(projectDir, _relativeOutputDirectory);
        Directory.CreateDirectory(outputDir);
        
        var filePath = Path.Combine(outputDir, fileName);
        bitmap.Save(filePath, format ?? ImageFormat.Png);
        
        return Path.GetFullPath(filePath);
    }

    private string GetProjectDirectory()
    {
        var baseDir = AppDomain.CurrentDomain.BaseDirectory;
        var currentDir = baseDir;
        while (currentDir != null)
        {
            if (Directory.GetFiles(currentDir, "*.csproj").Any())
                return currentDir;
            var parent = Directory.GetParent(currentDir);
            currentDir = parent?.FullName;
        }
        
        return baseDir;
    }
}