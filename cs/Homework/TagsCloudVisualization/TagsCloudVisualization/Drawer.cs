using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace TagsCloudVisualization
{
    public static class Drawer
    {
        public static void DrawAndSaveRectangles(Size canvasSize, HashSet<Rectangle> rectangles, string name, string path = "")
        {
            var bitmap = new Bitmap(canvasSize.Height, canvasSize.Width);
            var graphics = Graphics.FromImage(bitmap);
            graphics.FillRectangle(Brushes.White, 0, 0, 1000, 1000);
            foreach (var rect in rectangles)
                graphics.DrawRectangle(Pens.Black, rect);

            bitmap.Save($"{path}{name}", ImageFormat.Png);
            bitmap.Dispose();
        }
    }
}
