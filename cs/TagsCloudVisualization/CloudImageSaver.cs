using System.Drawing;
using System.Drawing.Imaging;

namespace TagsCloudVisualization;

public class CloudImageSaver
{
    private const string BaseDirectory = @"..\..\..\";

    public static void SaveFailedTest(Bitmap bitmap, string name)
    {
        var fileName = name + ".bmp";
        Save(bitmap, "TestFailedImages",  fileName);
    }

    public static void Save(Bitmap bitmap, string directoryName, string fileName)
    {
        var directory = Path.Combine(BaseDirectory, directoryName);
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
        
        bitmap.Save(Path.Combine(directory, fileName), ImageFormat.Bmp);
        Console.WriteLine("Tag cloud visualization saved to file " + Path.Combine(directoryName, fileName));
    }
}