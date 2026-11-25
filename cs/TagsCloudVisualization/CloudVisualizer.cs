using System.Drawing;
using System.Drawing.Imaging;

namespace TagsCloudVisualization;

public static class CloudVisualizer
{
    private const int ImagePadding = 20;
    private const int CenterPointRadius = 3;
    private const int ColorAlpha = 150;

    public static void SaveToFile(IEnumerable<Rectangle> rectangles, Point center, string path)
    {
        var rectangleList = rectangles.ToList();
        if (rectangleList.Count == 0)
            throw new ArgumentException("Cannot visualize an empty collection of rectangles", nameof(rectangles));

        var bounds = GetCloudBounds(rectangleList, ImagePadding);

        using var bitmap = new Bitmap(bounds.Width, bounds.Height);
        using var graphics = Graphics.FromImage(bitmap);
        graphics.Clear(Color.White);

        DrawCenter(graphics, center, bounds.Location);
        DrawRectangles(graphics, rectangleList, bounds.Location);

        bitmap.Save(path, ImageFormat.Png);
    }

    private static Rectangle GetCloudBounds(IReadOnlyList<Rectangle> rectangles, int padding)
    {
        var minX = rectangles.Min(r => r.Left) - padding;
        var minY = rectangles.Min(r => r.Top) - padding;
        var maxX = rectangles.Max(r => r.Right) + padding;
        var maxY = rectangles.Max(r => r.Bottom) + padding;

        return new Rectangle(minX, minY, maxX - minX, maxY - minY);
    }

    private static void DrawCenter(Graphics graphics, Point center, Point offset)
    {
        using var brush = new SolidBrush(Color.Red);
        graphics.FillEllipse(
            brush,
            center.X - offset.X - CenterPointRadius,
            center.Y - offset.Y - CenterPointRadius,
            CenterPointRadius * 2,
            CenterPointRadius * 2);
    }

    private static void DrawRectangles(Graphics graphics, IReadOnlyList<Rectangle> rectangles, Point offset)
    {
        var random = new Random();

        foreach (var rectangle in rectangles)
        {
            var shiftedRect = new Rectangle(
                rectangle.X - offset.X,
                rectangle.Y - offset.Y,
                rectangle.Width,
                rectangle.Height);

            DrawRectangle(graphics, shiftedRect, random);
        }
    }

    private static void DrawRectangle(Graphics graphics, Rectangle rect, Random random)
    {
        var color = Color.FromArgb(
            ColorAlpha,
            random.Next(256),
            random.Next(256),
            random.Next(256));

        using var fillBrush = new SolidBrush(color);
        graphics.FillRectangle(fillBrush, rect);

        using var pen = new Pen(Color.Black, 1);
        graphics.DrawRectangle(pen, rect);
    }
}