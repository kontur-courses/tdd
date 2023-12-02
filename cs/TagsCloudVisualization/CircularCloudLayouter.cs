using System.Drawing;
using Utility;

namespace TagsCloudVisualization;

public class CircularCloudLayouter(Dictionary<string, int> frequencyDict)
{
    private readonly Point center = new(960, 540);
    private readonly Size resolution = new(1920, 1080);

    private readonly SortedDictionary<string, int> frequencyDict = new(frequencyDict);

    private readonly List<Rectangle> rectangles = new();

    public List<Rectangle> Algorithm()
    {
        for (var radius = 0;
             radius < 0.8 * Math.Min(resolution.Width / 2, resolution.Height / 2)
             && frequencyDict.Count != 0;
             radius++)
        {
            for (var angle = 0; angle < 360 && frequencyDict.Count != 0; angle++)
            {
                var coordinate = PolarMath.PolarToCartesian(radius, angle, center);

                PutNextRectangle(coordinate);
            }
        }

        return rectangles;
    }

    public bool PutNextRectangle(Point coordinate)
    {
        foreach (var pair in frequencyDict.Reverse())
        {
            var size = GetSizeFromWordWithFrequency(pair.Key, pair.Value);

            var target = new Rectangle(coordinate, size);

            if (!IntersectWithPlaced(rectangles, target))
            {
                rectangles.Add(target);
                frequencyDict.Remove(pair.Key);
                return true;
            }
        }

        return false;
    }

    public static bool IntersectWithPlaced(List<Rectangle> rectangles, Rectangle target)
    {
        return rectangles.Any(target.IntersectsWith);
    }

    private static Size GetSizeFromWordWithFrequency(string word, int frequency)
    {
        return new Size(
            (10 + frequency) * word.Length,
            15 + frequency
        );
    }
}