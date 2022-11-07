using System.Diagnostics.CodeAnalysis;
using System.Drawing;

namespace TagsCloudVisualization;

[SuppressMessage("Interoperability", "CA1416:Проверка совместимости платформы")]
public static class ImageGenerator
{
    private static readonly Color[] Colors =
    {
        Color.Red,
        Color.Orange,
        Color.Yellow,
        Color.Green,
        Color.LightBlue,
        Color.Blue,
        Color.Purple
    };

    public static Bitmap Generate(Rectangle[] rectangles, Point center)
    {
        var canvasSize = GetCanvasSize(rectangles);
        var canvas = new Bitmap(canvasSize.Width, canvasSize.Height);
        using var graphics = Graphics.FromImage(canvas);

        graphics.FillRectangle(new SolidBrush(Color.White), new Rectangle(Point.Empty, canvasSize));
        rectangles = ToCenterOfImage(rectangles, center, canvas);
        DrawRectangles(graphics, rectangles);
        return canvas;
    }

    private static Size GetCanvasSize(Rectangle[] rectangles)
    {
        var offset = 1000;
        var maxX = rectangles.Max(r => r.Right);
        var maxY = rectangles.Max(r => r.Bottom);
        var minX = rectangles.Min(r => r.Left);
        var minY = rectangles.Min(r => r.Top);
        return new Size(maxX - minX + offset, maxY - minY + offset);
    }

    private static void DrawRectangles(Graphics graphics, IEnumerable<Rectangle> rectangles)
    {
        var index = 0;
        using var pen = new Pen(Colors[index++ % Colors.Length]);
        foreach (var rectangle in rectangles)
        {
            pen.Color = Colors[index++ % Colors.Length];
            graphics.DrawRectangle(pen, rectangle);
        }
    }

    private static Rectangle[] ToCenterOfImage(Rectangle[] rectangles, Point center, Bitmap image)
    {
        var shiftX = image.Width / 2 - center.X;
        var shiftY = image.Height / 2 - center.Y;

        return rectangles
            .Select(rectangle => new Rectangle(new Point(rectangle.X + shiftX, rectangle.Y + shiftY), rectangle.Size))
            .ToArray();
    }
}