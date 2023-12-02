using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using Utility;
using Point = System.Drawing.Point;
using Rectangle = System.Drawing.Rectangle;
using Size = System.Drawing.Size;

namespace TagsCloudVisualization;

public class CircularCloudLayouter(Dictionary<string, int> frequencyDict)
{
    private readonly Point center = new(960, 540);
    public Dictionary<string, Rectangle> PlacedWords { get; } = new();
    private readonly Image<Rgba32> image = new(1920, 1080);

    public void GenerateTagCloud(string outputName)
    {
        for (var radius = 0;
             radius < 0.8 * Math.Min(image.Width / 2, image.Height / 2)
             && frequencyDict.Count != 0;
             radius++)
        {
            for (var angle = 0; angle < 360 && frequencyDict.Count != 0; angle++)
            {
                var coordinate = PolarMath.PolarToCartesian(radius, angle, center);

                PutNextRectangle(coordinate);
            }
        }

        var encoder = new JpegEncoder { Quality = 100 };

        image.Save($"../../../../TagsCloudVisualization/{outputName}.jpg", encoder);
        image.Dispose();
    }

    public void PutNextRectangle(Point coordinate, bool isTest = false)
    {
        foreach (var kvp in frequencyDict.OrderByDescending(kvp => kvp.Value))
        {
            var size = GetSizeFromWordWithFrequency(kvp.Key, kvp.Value);

            var target = new Rectangle(coordinate, size);

            if (!IntersectWithPlaced(target))
            {
                PlacedWords.Add(kvp.Key, target);
                frequencyDict.Remove(kvp.Key);

                if (!isTest)
                {
                    var rectangle = new RectangleF(target.X, target.Y, target.Width, target.Height);

                    const string fontName = "JosefinSans-Regular";

                    FontCollection collection = new();
                    var family = collection.Add($"../../../../TagsCloudVisualization/{fontName}.ttf");
                    var font = family.CreateFont(14 + kvp.Value, FontStyle.Italic);

                    // image.Mutate(x => x.Draw(Color.White, 2f, rectangle));

                    image.Mutate(x => x.DrawText(
                        kvp.Key, font, Color.FromRgba(64,224,208, (byte)Math.Min(255, 200 + kvp.Value)), new PointF(target.X, target.Y))
                    );
                }
            }
        }
    }

    public bool IntersectWithPlaced(Rectangle target)
    {
        var tmpRectangle = new Rectangle(target.X - 1, target.Y - 1, target.Width + 2, target.Height + 2);
        return PlacedWords.Values.ToList().Any(tmpRectangle.IntersectsWith);
    }

    private static Size GetSizeFromWordWithFrequency(string word, int frequency)
    {
        return new Size(
            (10 + frequency) * word.Length,
            15 + frequency
        );
    }
}