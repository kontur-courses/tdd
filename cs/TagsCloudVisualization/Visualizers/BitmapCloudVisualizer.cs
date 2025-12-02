using System.Drawing;

namespace TagsCloudVisualization.Visualizers;

public class BitmapCloudVisualizer
{
    private readonly VisualizerSettings settings;
    private const double ScaleFactor = 0.8;

    public BitmapCloudVisualizer(VisualizerSettings settings)
    {
        this.settings = settings;
    }

    public Image CreateImage(IEnumerable<Rectangle> rectangles)
    {
        var bitmap = new Bitmap(settings.Width, settings.Height);
        using var graphics = Graphics.FromImage(bitmap);

        graphics.Clear(settings.BackgroundColor);

        if (!rectangles.Any())
            return bitmap;

        var (scale, offsetX, offsetY) = CalculateTransformCoefficients(rectangles);
        DrawRectangles(graphics, rectangles, scale, offsetX, offsetY);

        return bitmap;
    }

    private (double scale, double offsetX, double offsetY) CalculateTransformCoefficients(
        IEnumerable<Rectangle> rectangles)
    {
        var minX = rectangles.Min(r => r.Left);
        var maxX = rectangles.Max(r => r.Right);
        var minY = rectangles.Min(r => r.Top);
        var maxY = rectangles.Max(r => r.Bottom);

        var cloudWidth = maxX - minX;
        var cloudHeight = maxY - minY;

        var scaleX = settings.Width * ScaleFactor / cloudWidth;
        var scaleY = settings.Height * ScaleFactor / cloudHeight;
        var scale = Math.Min(scaleX, scaleY);

        var offsetX = (settings.Width - cloudWidth * scale) / 2 - minX * scale;
        var offsetY = (settings.Height - cloudHeight * scale) / 2 - minY * scale;

        return (scale, offsetX, offsetY);
    }

    private void DrawRectangles(Graphics graphics, IEnumerable<Rectangle> rectangles, double scale, double offsetX,
        double offsetY)
    {
        using var pen = new Pen(settings.RectangleBorderColor, settings.RectangleBorderWidth);
        using var brush = new SolidBrush(settings.RectangleColor);

        foreach (var rectangle in rectangles)
        {
            var rectangleToDraw = new Rectangle(
                (int)(offsetX + rectangle.Left * scale),
                (int)(offsetY + rectangle.Top * scale),
                (int)(rectangle.Width * scale),
                (int)(rectangle.Height * scale));

            graphics.DrawRectangle(pen, rectangleToDraw);
            graphics.FillRectangle(brush, rectangleToDraw);
        }
    }
}