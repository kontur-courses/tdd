using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
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
    private readonly Size resolution = new(1920, 1080);

    private readonly SortedDictionary<string, int> frequencyDict = new(frequencyDict);

    private readonly Dictionary<string, Rectangle> placedWords = new();

    public Dictionary<string, Rectangle> Algorithm()
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

        return placedWords;
    }

    public bool PutNextRectangle(Point coordinate)
    {
        foreach (var pair in frequencyDict.Reverse())
        {
            var size = GetSizeFromWordWithFrequency(pair.Key, pair.Value);

            var target = new Rectangle(coordinate, size);

            if (!IntersectWithPlaced(placedWords, target))
            {
                placedWords.Add(pair.Key, target);
                frequencyDict.Remove(pair.Key);
                return true;
            }
        }

        return false;
    }

    public static bool IntersectWithPlaced(Dictionary<string, Rectangle> rectanglesWithWords, Rectangle target)
    {
        return rectanglesWithWords.Values.ToList().Any(target.IntersectsWith);
    }

    private static Size GetSizeFromWordWithFrequency(string word, int frequency)
    {
        return new Size(
            (10 + frequency) * word.Length,
            15 + frequency
        );
    }

    public void CreatePicture()
    {
        using Image<Rgba32> image = new(resolution.Width, resolution.Height);

        foreach (var placed in placedWords.Values)
        {
            var rectangle = new RectangleF(placed.X, placed.Y, placed.Width, placed.Height);
            
            image.Mutate( x=> x.Fill(Color.White, rectangle));
        }
        
        image.Save("../../../../TagsCloudVisualization/rectangles.jpg");
    }
}