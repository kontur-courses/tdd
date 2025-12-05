using System.Drawing;

namespace TagsCloudVisualization;

public class TagCloudRenderer(Size imageSize) : ICloudRenderer
{
    public Bitmap CreateRectangleCloud(IEnumerable<Rectangle> rectangles)
    {
        var bitmap = new Bitmap(imageSize.Width, imageSize.Height);

        using var graphics = Graphics.FromImage(bitmap);
        graphics.Clear(Color.FromArgb(0, 34, 43));
        var pen = new Pen(Color.FromArgb(212, 85, 0), 2);
        foreach (var rect in rectangles)
            graphics.DrawRectangle(pen, rect);

        return bitmap;
    }
}