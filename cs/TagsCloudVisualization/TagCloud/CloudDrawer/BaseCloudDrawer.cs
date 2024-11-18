using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;

namespace TagsCloudVisualization.TagCloud;

public class BaseCloudDrawer : ICloudDrawer
{
    public Bitmap DrawCloud(List<Rectangle> rectangles, int imageWidth, int imageHeight)
    {
        if (imageWidth <= 0)
            throw new ArgumentException("Width must be positive number");
        if (imageHeight <= 0)
            throw new ArgumentException("Height must be positive number");
        var bitmap = new Bitmap(imageWidth, imageHeight);
        var graphics = Graphics.FromImage(bitmap);
        graphics.Clear(Color.White);
        foreach (var rectangle in rectangles)
        {
            graphics.DrawRectangle(new Pen(Color.Black), rectangle);
        }
        
        return bitmap;
    }

    public void SaveToFile(Bitmap bitmap, string? fileName = null, string? path = null, ImageFormat format = null)
    {
        path ??= Environment.CurrentDirectory;
        fileName ??= DateTime.Now.ToOADate().ToString(CultureInfo.InvariantCulture);
        format ??= ImageFormat.Png;
        var fullPath = Path.Combine(path, fileName + ".png");
        bitmap.Save(fullPath);
    }
}