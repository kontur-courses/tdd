using System.Drawing;
using System.Collections.Generic;
using System.Drawing.Imaging;

namespace TagsCloudVisualization;

public class CloudDrawer
{
    public static void DrawAndSaveCloud(Rectangle[] rectangles, string fileName)
    {
        var bitmap = new Bitmap(500, 500);
        var gr = Graphics.FromImage(bitmap);
        gr.FillRectangle(Brushes.Black, 0, 0, 500, 500);
        gr.FillRectangles(Brushes.White, rectangles);
        bitmap.Save(fileName);
    }
}