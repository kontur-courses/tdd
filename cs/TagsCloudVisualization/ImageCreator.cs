using System.Drawing;

namespace TagsCloudVisualization;

public class ImageCreator
{
    private readonly ImageOptions options;

    public ImageCreator(ImageOptions options)
    {
        this.options = options;
    }

    public Bitmap CreateImage(List<Rectangle> rectangles)
    {
        var bitmap = new Bitmap(options.ImageSize.Width, options.ImageSize.Height);
        var graphics = Graphics.FromImage(bitmap);
        graphics.Clear(options.BackgroundColor);

        using var pen = new Pen(options.RectangleBorderColor, 2);
        using var brush = new SolidBrush(options.RectangleFillColor);
        var offsetX = options.ImageSize.Width / 2;
        var offsetY = options.ImageSize.Height / 2;

        foreach (var rect in rectangles)
        {
            var shiftedRect = rect with { X = rect.X + offsetX, Y = rect.Y + offsetY };
            graphics.FillRectangle(brush, shiftedRect);
            graphics.DrawRectangle(pen, shiftedRect);
        }

        return bitmap;
    }
}