using System.Drawing;
using Utility;

namespace TagsCloudVisualization;

public class CircularCloudLayouter(Dictionary<string, int> frequencyDict)
{
    private readonly Point center = new(960, 540);
    public Dictionary<string, Rectangle> PlacedWords { get; } = new();
    private readonly Dictionary<string, int> frequencyDict = new(frequencyDict);
    private readonly Size resolution = new(1920, 1080);

    private const int FontSize = 30;

    public void GenerateTagCloud(string outputName)
    {
        var rnd = new Random();
        for (var radius = 0;
             radius < 0.9 * Math.Min(resolution.Width / 2, resolution.Height / 2)
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

        ImageGenerator.GenerateImage(PlacedWords, outputName, FontSize);
    }

    public void PutNextRectangle(Point coordinate)
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