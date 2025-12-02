using System.Drawing;

namespace CloudLayouterVisualizer;

public class CloudVisualizer(List<Rectangle> rectangles, string filePath, double scale, int padding)
{
    private static readonly Color BackgroundColor = Color.White;
    private static readonly Color RectangleFillColor = Color.Orange;
    private static readonly Color RectangleBorderColor = Color.Black;

    private const float RectangleBorderThickness = 1f;
    private const string FontFamilyName = "Arial";
    private const float FontSize = 8f;

    public void Visualize()
    {
        var minX = rectangles.Min(rect => rect.Left);
        var minY = rectangles.Min(rect => rect.Top);
        var maxX = rectangles.Max(rect => rect.Right);
        var maxY = rectangles.Max(rect => rect.Bottom);

        var cloudWidth = maxX - minX;
        var cloudHeight = maxY - minY;

        var imageWidth = (int)Math.Ceiling(cloudWidth * scale + 2 * padding);
        var imageHeight = (int)Math.Ceiling(cloudHeight * scale + 2 * padding);

        using var bitmap = new Bitmap(imageWidth, imageHeight);
        using var graphics = Graphics.FromImage(bitmap);

        graphics.Clear(BackgroundColor);

        using var fillBrush = new SolidBrush(RectangleFillColor);
        using var font = new Font(FontFamilyName, FontSize);
        using var pen = new Pen(RectangleBorderColor, RectangleBorderThickness);

        foreach (var rect in rectangles)
        {
            var x = (float)((rect.X - minX) * scale + padding);
            var y = (float)((rect.Y - minY) * scale + padding);
            var width = (float)(rect.Width * scale);
            var height = (float)(rect.Height * scale);

            var imageRect = new RectangleF(x, y, width, height);

            graphics.FillRectangle(fillBrush, imageRect);
            graphics.DrawRectangle(pen, imageRect.X, imageRect.Y, imageRect.Width, imageRect.Height);
        }

        bitmap.Save(filePath);
    }
}
