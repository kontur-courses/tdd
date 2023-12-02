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
    private readonly Dictionary<string, int> frequencyDict = new(frequencyDict);
    private readonly Image<Rgba32> image = new(1920, 1080);

    private const int FontSize = 30;
    private const string FontPath = "src/JosefinSans-Regular";

    public void GenerateTagCloud(string outputName)
    {
        // Задаём фон изображения
        image.Mutate(x => x.Fill(Color.FromRgb(7, 42, 22)));

        var rnd = new Random();
        for (var radius = 0;
             radius < 0.9 * Math.Min(image.Width / 2, image.Height / 2)
             && frequencyDict.Count != 0;
             radius += FontSize)
        {
            // Угол рандомизирую для менее детерминированного распределения, i - счётчик для поворота только на 360
            var i = 0;
            for (var angle = rnd.Next(360); i < 360 && frequencyDict.Count != 0; angle++, i++)
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

            var centeredCoordinate = new Point(coordinate.X - size.Width / 2, coordinate.Y - size.Height / 2);
            var target = new Rectangle(centeredCoordinate, size);

            if (!IntersectWithPlaced(target))
            {
                PlacedWords.Add(kvp.Key, target);
                frequencyDict.Remove(kvp.Key);

                if (!isTest)
                {
                    FontCollection collection = new();
                    var family = collection.Add($"../../../../TagsCloudVisualization/{FontPath}.ttf");
                    var font = family.CreateFont(FontSize + kvp.Value, FontStyle.Italic);

                    // Рисование прямоугольников
                    // var rectangle = new RectangleF(target.X, target.Y, target.Width, target.Height);
                    // image.Mutate(x => x.Draw(Color.Black, 2f, rectangle));

                    image.Mutate(x => x.DrawText(
                        kvp.Key, font,
                        Color.FromRgba(211, 226, 157, (byte)Math.Min(255, 100 + kvp.Value * 10)),
                        new PointF(target.X, target.Y))
                    );
                }

                return;
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
            (int)((FontSize + frequency) * word.Length / 1.5),
            FontSize + frequency
        );
    }
}