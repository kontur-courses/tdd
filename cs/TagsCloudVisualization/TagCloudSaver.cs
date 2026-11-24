using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.Versioning;

namespace TagsCloudVisualization;

public static class TagCloudSaver
{
    [SupportedOSPlatform("windows")]
    public static void Save(CircularCloudLayouter tagCloud, string path, int x, int y)
    {
        using var bitmap = new Bitmap(x, y);
        using var graphics = Graphics.FromImage(bitmap);
        graphics.Clear(Color.White);
        foreach (var rectangle in tagCloud.cloudRectangles)
        {
            if (rectangle.Width >= 2 && rectangle.Height >= 2)
            {
                graphics.DrawRectangle(Pens.Black, rectangle.X, rectangle.Y, rectangle.Width - 1, rectangle.Height - 1);
            }
            else
            {
                graphics.FillRectangle(Brushes.Black, rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
            }
        }
        graphics.FillRectangle(Brushes.Red, tagCloud.center.X, tagCloud.center.Y, 1, 1);
        bitmap.Save(path, ImageFormat.Png);
    }
}
