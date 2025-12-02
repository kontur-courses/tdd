using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using DrawingRectangle = System.Drawing.Rectangle;

namespace TagCloud.Visualisation;

public class CloudVisualizer(CloudVisualizationConfig config)
{
    private readonly CloudVisualizationConfig config = config ?? throw new ArgumentNullException(nameof(config));

    public void DrawLayout(IEnumerable<DrawingRectangle> rectangles, string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
            throw new ArgumentException("Некорректное имя файла", nameof(fileName));

        using var image = new Image<Rgba32>(config.Width, config.Height, config.BackgroundColor);

        var rectList = new List<DrawingRectangle>(rectangles);
        if (rectList.Count == 0)
            throw new ArgumentException("Коллекция прямоугольников пуста", nameof(rectangles));

        for (var i = 0; i < rectList.Count; i++)
        {
            var original = rectList[i];
            var translated = TranslateRectangle(original);
            var fillColor = config.RectangleColorProvider(original, i);
            DrawFilledRectangle(image, translated, fillColor);
        }

        var projectRoot = Path.GetFullPath(
            Path.Combine(AppContext.BaseDirectory, "..", "..", ".."));
        var outputPath = Path.Combine(projectRoot, fileName);

        image.Save(outputPath);
    }

    private DrawingRectangle TranslateRectangle(DrawingRectangle rect)
    {
        var dx = config.Center.X;
        var dy = config.Center.Y;

        return rect with { X = rect.X + dx, Y = rect.Y + dy };
    }

    private static void DrawFilledRectangle(Image<Rgba32> image, DrawingRectangle rect, Rgba32 fillColor)
    {
        var borderColor = new Rgba32(0, 0, 0, 255);

        var width = image.Width;
        var height = image.Height;
        
        var left   = Math.Max(rect.Left, 0);
        var right  = Math.Min(rect.Right - 1, width - 1);
        var top    = Math.Max(rect.Top, 0);
        var bottom = Math.Min(rect.Bottom - 1, height - 1);
        
        if (left > right || top > bottom)
            return;
        
        for (var y = top; y <= bottom; y++)
        for (var x = left; x <= right; x++)
            image[x, y] = fillColor;
        
        DrawLine(left, right, x =>
        {
            image[x, top] = borderColor;
            image[x, bottom] = borderColor;
        });
        
        DrawLine(top, bottom, y =>
        {
            image[left, y] = borderColor;
            image[right, y] = borderColor;
        });
    }

    private static void DrawLine(int fromInclusive, int toInclusive, Action<int> putPixel)
    {
        for (var i = fromInclusive; i <= toInclusive; i++)
            putPixel(i);
    }
}
