using System.Drawing;
using System.Drawing.Imaging;

namespace TagsCloudVisualizationApp;

public static class CloudVisualizer
{
    public static void DrawRectangles(IEnumerable<Rectangle> rectangles, Point center, string filename = "cloud.bmp")
    {
        var bmp = new Bitmap(800, 800);
        var graphics = Graphics.FromImage(bmp);
        graphics.Clear(Color.White);
        
        foreach (var rect in rectangles)
        {
            graphics.FillRectangle(Brushes.LightBlue, rect);
            graphics.DrawRectangle(Pens.Black, rect);
        }
        graphics.FillEllipse(Brushes.Red, center.X, center.Y, 5, 5);
        
        bmp.Save(filename, ImageFormat.Bmp);
    }
}