using System.Drawing;

namespace TagCloud;

public static class CircularCloudLayoutExtensions
{
    public static void SaveAsImage(this CircularCloudLayouter layouter, string filename, Size imageSize)
    {
        using var bitmap = new Bitmap(imageSize.Width, imageSize.Height);
        using var graphics = Graphics.FromImage(bitmap);
        var pen = new Pen(Color.Black, 1);

        graphics.Clear(Color.White);
        foreach (var rectangle in layouter.Rectangles)
            graphics.DrawRectangle(pen, rectangle);

        bitmap.Save(filename);
    }
}