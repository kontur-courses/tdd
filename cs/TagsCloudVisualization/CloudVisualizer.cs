using System.Drawing;
using System.Drawing.Imaging;

namespace TagCloud;

public static class CloudVisualizer
{
    public static void DrawRectangles(IEnumerable<Rectangle> rectangles, string filename, int bitmapWidth, int bitmapHeight)
    {
        var bitmap = new Bitmap(bitmapWidth, bitmapHeight);
        var graphics = Graphics.FromImage(bitmap);

        foreach (var rect in rectangles)
        {
            graphics.FillRectangle(Brushes.LightBlue, rect);
            graphics.DrawRectangle(Pens.Black, rect);
        }
        
        var projectRoot = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.FullName;
        var imagesPath = Path.Combine(projectRoot, "Images");
        Directory.CreateDirectory(imagesPath);
        bitmap.Save(Path.Combine(imagesPath, filename+".png"), ImageFormat.Png);
    }
}