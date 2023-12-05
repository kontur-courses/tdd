using System.Drawing;

namespace TagsCloudVizualization;

public static class Visualizer
{
    public static Bitmap VisualizeRectangles(IList<Rectangle> rectangles, int bitmapWidth, int bitmapHeight)
    {
        var bitmap = new Bitmap(bitmapWidth, bitmapHeight);
        using var graphics = Graphics.FromImage(bitmap);

        DrawRectangles(rectangles, graphics);

        return bitmap;
    }
    
    private static void DrawRectangles(IEnumerable<Rectangle> rectangles, Graphics graphics)
    {
        var pen = new Pen(Color.Green);
        foreach (var rect in rectangles)
        {
            graphics.DrawRectangle(pen, rect);
        }
    }

    public static void SaveBitmap(Bitmap bitmap, string fileName, string pathToDirectory)
    {
        EnsureDirectoryExists(pathToDirectory);

        var safeFileName = GetSafeFileName(fileName);
        bitmap.Save(Path.Combine(pathToDirectory, safeFileName), System.Drawing.Imaging.ImageFormat.Png);
    }

    private static void EnsureDirectoryExists(string directory)
    {
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
    }

    private static string GetSafeFileName(string fileName)
    {
        var invalidChars = Path.GetInvalidFileNameChars();
        return string.Concat(fileName.Split(invalidChars, StringSplitOptions.RemoveEmptyEntries));
    }
}