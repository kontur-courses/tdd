using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace TagsCloudVisualization;

public static class ImageGenerator
{
    private const string FontPath = "src/JosefinSans-Regular";

    public static void GenerateImage(Dictionary<string, System.Drawing.Rectangle> placedWords,
        string outputName, int fontSize)
    {
        Image<Rgba32> image = new(1920, 1080);

        // Задаём фон изображения
        image.Mutate(x => x.Fill(Color.FromRgb(7, 42, 22)));

        foreach (var kvp in placedWords)
        {
            var frequency = kvp.Value.Height - fontSize;

            FontCollection collection = new();
            var family = collection.Add($"../../../../TagsCloudVisualization/{FontPath}.ttf");
            var font = family.CreateFont(fontSize + frequency, FontStyle.Italic);

            // Рисование прямоугольников
            // var rectangle = new RectangleF(target.X, target.Y, target.Width, target.Height);
            // image.Mutate(x => x.Draw(Color.Black, 2f, rectangle));

            image.Mutate(x => x.DrawText(
                kvp.Key, font,
                Color.FromRgba(211, 226, 157, (byte)Math.Min(255, 100 + frequency * 10)),
                new PointF(kvp.Value.X, kvp.Value.Y))
            );
        }

        var encoder = new JpegEncoder { Quality = 100 };

        image.Save($"../../../../TagsCloudVisualization/{outputName}.jpg", encoder);
        image.Dispose();
    }
}