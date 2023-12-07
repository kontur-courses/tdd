using System.Drawing;
using System.Drawing.Imaging;

namespace TagsCloudVisualization;

public class TagsCloudDrawer
{
    private readonly ICloudLayouter layouter;
    private readonly Pen pen;
    private readonly int scale;


    public TagsCloudDrawer(ICloudLayouter layouter, Pen pen, int scale)
    {
        this.layouter = layouter;
        if (scale <= 0)
            throw new ArgumentException("Scale must be a positive number.");
        this.pen = pen ?? throw new ArgumentException("Pen must not be null.");
        this.scale = scale;
    }

    private static ImageParameters AdjustImageParameters(Rectangle borders)
    {
        var width = borders.Width;
        var height = borders.Height;
        var offsetX = 0;
        var offsetY = 0;
        if (borders.Left < 0)
        {
            width += Math.Abs(borders.Left);
            offsetX = borders.Left;
        }

        if (borders.Top < 0)
        {
            height += Math.Abs(borders.Top);
            offsetY = borders.Top;
        }

        if (borders.Right > width)
            width += borders.Right - width;

        if (borders.Bottom > height)
            height += borders.Bottom - height;

        return new ImageParameters(offsetX, offsetY, height, width);
    }

    public Bitmap DrawTagCloud()
    {
        if (layouter.PlacedRectangles is null)
            throw new ArgumentException("The list of rectangles cannot be null");

        var borders = layouter.PlacedRectangles.GetBorders();
        var adjustedSize = AdjustImageParameters(borders);

        var bitmap = new Bitmap(adjustedSize.Width * scale, adjustedSize.Height * scale);
        var graphics = Graphics.FromImage(bitmap);
        graphics.TranslateTransform(-adjustedSize.OffsetX * scale, -adjustedSize.OffsetY * scale);

        foreach (var rectangle in layouter.PlacedRectangles.Select(r =>
                     new Rectangle(r.X * scale, r.Y * scale, r.Width * scale, r.Height * scale)))
            graphics.DrawRectangle(pen, rectangle);

        return bitmap;
    }

    public static void SaveImage(Bitmap bitmap, string dirPath, string filename)
    {
        if (string.IsNullOrWhiteSpace(filename) || filename.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
            throw new ArgumentException("The provided filename is not valid.");

        try
        {
            Directory.CreateDirectory(dirPath);
        }
        catch (Exception)
        {
            throw new ArgumentException("The provided directory path is not valid.");
        }

        bitmap.Save(Path.Combine(dirPath, filename), ImageFormat.Jpeg);
    }
}