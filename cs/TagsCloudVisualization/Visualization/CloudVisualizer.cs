using System.Diagnostics.CodeAnalysis;
using System.Drawing;

namespace TagsCloudVisualization.Visualization;

public static class CloudVisualizer
{
    [SuppressMessage("Interoperability", "CA1416:Проверка совместимости платформы")]
    public static void SaveImage(IReadOnlyList<Rectangle> rectangles, string path,
        Color backgroundColor = default, Color rectangleColor = default)
    {
        if (rectangles.Count == 0)
            throw new ArgumentException("Cannot visualize an empty list of rectangles.", nameof(rectangles));

        var minX = rectangles.Min(r => r.Left);
        var minY = rectangles.Min(r => r.Top);
        var width = rectangles.Max(r => r.Right) - minX;
        var height = rectangles.Max(r => r.Bottom) - minY;

        using var bitmap = new Bitmap(width + 1, height + 1);
        using var graphics = Graphics.FromImage(bitmap);
        graphics.Clear(backgroundColor == default ? Color.White : backgroundColor);

        using var rectangleBrush = new SolidBrush(rectangleColor == default ? Color.LightBlue : rectangleColor);
        for (var i = 0; i < rectangles.Count; i++)
        {
            var r = rectangles[i];
            if (rectangleColor == default)
                rectangleBrush.Color = GetLightBlueShade(i);

            graphics.FillRectangle(rectangleBrush, r.X - minX, r.Y - minY, r.Width, r.Height);
            graphics.DrawRectangle(Pens.Black, r.X - minX, r.Y - minY, r.Width, r.Height);
        }

        bitmap.Save(path);
    }

    private static Color GetLightBlueShade(int index)
    {
        const int shadeCount = 41;
        const int shadeStep = 17;
        const int saturationCount = 17;
        var offset = (index % shadeCount * shadeStep + shadeCount / 2) % shadeCount - shadeCount / 2;
        var saturation = (index / shadeCount + saturationCount / 2) % saturationCount - saturationCount / 2;
        var blue = Color.LightBlue;

        return Color.FromArgb(
            Math.Clamp(blue.R + offset - saturation, 0, 255),
            Math.Clamp(blue.G + offset + saturation / 2, 0, 255),
            Math.Clamp(blue.B + offset + saturation, 0, 255));
    }
}
