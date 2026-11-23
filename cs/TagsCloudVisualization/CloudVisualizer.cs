using System.Drawing;
using SkiaSharp;

namespace TagsCloudVisualization;

public class CloudVisualizer
{
    public static void SaveToFile(IEnumerable<Rectangle> rectangles, Point center, string path)
    {
        var rectangleList = rectangles.ToList();
        if (!rectangleList.Any())
            return;
        
        int minX = rectangleList.Min(r => r.Left) - 20;
        int minY = rectangleList.Min(r => r.Top) - 20;
        int maxX = rectangleList.Max(r => r.Right) + 20;
        int maxY = rectangleList.Max(r => r.Bottom) + 20;
        int width = maxX - minX;
        int height = maxY - minY;

        using var bitmap = new SKBitmap(width, height);
        using var canvas = new SKCanvas(bitmap);

        canvas.Clear(SKColors.White);

        using var centerPaint = new SKPaint
        {
            Color = SKColors.Red,
            Style = SKPaintStyle.Fill,
            IsAntialias = true
        };
        canvas.DrawCircle(center.X - minX, center.Y - minY, 3, centerPaint);

        var random = new Random();

        foreach (var rectangle in rectangleList)
        {
            var shiftedX = rectangle.X - minX;
            var shiftedY = rectangle.Y - minY;
            var rectSK = new SKRect(shiftedX, shiftedY, shiftedX + rectangle.Width, shiftedY + rectangle.Height);

            var color = new SKColor(
                (byte)random.Next(256),
                (byte)random.Next(256),
                (byte)random.Next(256),
                150);

            using var fillPaint = new SKPaint
            {
                Color = color,
                Style = SKPaintStyle.Fill,
                IsAntialias = true
            };
            canvas.DrawRect(rectSK, fillPaint);

            using var strokePaint = new SKPaint
            {
                Color = SKColors.Black,
                Style = SKPaintStyle.Stroke,
                StrokeWidth = 1,
                IsAntialias = true
            };
            canvas.DrawRect(rectSK, strokePaint);
        }

        using var image = SKImage.FromBitmap(bitmap);
        using var data = image.Encode(SKEncodedImageFormat.Png, 100);
        using var stream = File.Create(path);
        data.SaveTo(stream);
    }
}