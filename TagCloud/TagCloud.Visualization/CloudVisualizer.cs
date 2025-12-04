using SkiaSharp;
using TagCloud.Shapes;

namespace TagCloud.Visualization;
public static class CloudVisualizer
{
    public static void SaveCloudImage(
        CircularCloudLayouter layouter,
        int width,
        int height,
        string filePath)
    {
        var info = new SKImageInfo(width, height);
        using var surface = SKSurface.Create(info);
        var canvas = surface.Canvas;

        canvas.Clear(SKColors.Azure);

        using var fill = new SKPaint();
        fill.Color = new SKColor(0, 255, 0, 40);
        fill.Style = SKPaintStyle.Fill;
        fill.IsAntialias = true;

        using var stroke = new SKPaint();
        stroke.Color = SKColors.DarkGreen;
        stroke.Style = SKPaintStyle.Stroke;
        stroke.StrokeWidth = 1;
        stroke.IsAntialias = true;

        foreach (var shape in layouter.Shapes.OfType<RectangleCloudShape>())
        {
            var rectangle = shape.BoundingBox;
            var rectangleToDraw = new SKRect(rectangle.Left, rectangle.Top, rectangle.Right, rectangle.Bottom);

            canvas.DrawRect(rectangleToDraw, fill);
            canvas.DrawRect(rectangleToDraw, stroke);
        }

        using var image = surface.Snapshot();
        using var data = image.Encode(SKEncodedImageFormat.Png, 100);
        using var stream = File.Open(filePath, FileMode.Create, FileAccess.Write);
        data.SaveTo(stream);
    }
}