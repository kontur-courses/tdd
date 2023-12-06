using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using Color = SixLabors.ImageSharp.Color;
using PointF = SixLabors.ImageSharp.PointF;
using Rectangle = System.Drawing.Rectangle;

namespace TagsCloudVisualization;

public class ImageGenerator : IDisposable
{
    private const string FontPath = "src/JosefinSans-Regular";
    private readonly Image<Rgba32> image;
    private readonly string outputName;
    private readonly int fontSize;
    private readonly FontFamily family;
    private readonly ImageEncoder encoder;

    public ImageGenerator(string outputName,
        int fontSize = 30, int width = 1920, int height = 1080, int quality = 100)
    {
        image = new Image<Rgba32>(width, height);
        this.outputName = $"../../../../TagsCloudVisualization/out/{outputName}.jpg";
        encoder = new JpegEncoder { Quality = quality };

        this.fontSize = fontSize;
        family = new FontCollection().Add($"../../../../TagsCloudVisualization/{FontPath}.ttf");

        SetBackground(Color.FromRgb(7, 42, 22));
    }

    private Font FontCreator(int size)
    {
        return family.CreateFont(size, FontStyle.Italic);
    }

    private void SetBackground(Color color)
    {
        image.Mutate(x => x.Fill(color));
    }

    public void DrawWord(string word, int frequency, Rectangle rectangle)
    {
        image.Mutate(x => x.DrawText(
            word, FontCreator(fontSize + frequency),
            Color.FromRgba(211, 226, 157, (byte)Math.Min(255, 100 + frequency * 10)),
            new PointF(rectangle.X, rectangle.Y))
        );
    }

    public System.Drawing.Size GetOutward(string word, int frequency)
    {
        var textOption = new TextOptions(FontCreator(fontSize + frequency));
        var size = TextMeasurer.MeasureSize(word, textOption);

        return new System.Drawing.Size((int)size.Width + fontSize / 3, (int)size.Height + fontSize / 3);
    }

    public void Dispose()
    {
        image.Save(outputName, encoder);
        image.Dispose();
    }
}