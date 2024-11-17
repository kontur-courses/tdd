using System.Drawing;
using System.Drawing.Imaging;

namespace TagsCloudVisualization.Visualizers;

public class SimpleCloudVisualizer
{
    private const string imagesDirectory = "images";

    public void CreateBitmap(
        IEnumerable<Rectangle> rectangles, 
        Size bitmapSize,
        string path = null
        )
    {
        var bitmap = new Bitmap(bitmapSize.Width, bitmapSize.Height);
        using (var graphics = Graphics.FromImage(bitmap))
        {
            graphics.Clear(Color.White);
            foreach (var rectangle in rectangles)
            {
                var pen = new Pen(GetRandomColor());
                graphics.DrawRectangle(pen, rectangle);
            }
            var currentPath = path == null ? GetPathToImages(rectangles.Count()) : path;
            Directory.CreateDirectory(Path.GetDirectoryName(currentPath));
            bitmap.Save((string)currentPath, ImageFormat.Jpeg);
        }
    }

    private static Color GetRandomColor()
    {
        var random = new Random();
        return Color.FromArgb(random.Next(0, 255), random.Next(0, 255), random.Next(0, 255));
    }

    private static string GetPathToImages(int rectanglesNumber)
    {
        var filename = $"{rectanglesNumber}_TagCloud.jpg";
        return Path.Combine(imagesDirectory, filename);
    }
}