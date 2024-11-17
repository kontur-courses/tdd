using System.Drawing;
using System.Drawing.Imaging;

namespace TagsCloudVisualization;

public class CloudImageRenderer
{
    private readonly Size imageSize;

    public CloudImageRenderer(Size imageSize)
    {
        this.imageSize = imageSize;
    }

    public void SaveToFile(string filename, IEnumerable<Rectangle> rectangles)
    {
        using var bitmap = new Bitmap(imageSize.Width, imageSize.Height);
        using var graphics = Graphics.FromImage(bitmap);
        graphics.Clear(Color.Black);

        var random = new Random();
        foreach (var rectangle in rectangles)
        {
            var color = Color.FromArgb(
                random.Next(128, 255),
                random.Next(128, 255),
                random.Next(128, 255)
            );

            using var brush = new SolidBrush(Color.FromArgb(180, color));
            using var pen = new Pen(color, 2f);

            var rect = new Rectangle(
                rectangle.X + imageSize.Width / 2,
                rectangle.Y + imageSize.Height / 2,
                rectangle.Width,
                rectangle.Height);

            graphics.FillRectangle(brush, rect);
            graphics.DrawRectangle(pen, rect);
        }

        bitmap.Save(filename, ImageFormat.Png);
    }
}