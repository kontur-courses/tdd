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

    private readonly SortedDictionary<string, int> frequencyDict = new(frequencyDict);

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
        
        image.Save($"../../../../TagsCloudVisualization/{outputName}.jpg");
        image.Dispose();
    }

    public bool PutNextRectangle(Point coordinate, bool isTest = false)
    {
        foreach (var pair in frequencyDict.Reverse())
        {
            var size = GetSizeFromWordWithFrequency(pair.Key, pair.Value);

            var target = new Rectangle(coordinate, size);

            if (!IntersectWithPlaced(target))
            {
                PlacedWords.Add(pair.Key, target);
                frequencyDict.Remove(pair.Key);
                
                if (!isTest)
                {
                    var rectangle = new RectangleF(target.X, target.Y, target.Width, target.Height);
                    image.Mutate( x=> x.Fill(Color.White, rectangle));
                }

                return true;
            }
        }

        return false;
    }

    public bool IntersectWithPlaced(Rectangle target)
    {
        return PlacedWords.Values.ToList().Any(target.IntersectsWith);
    }

    private static Size GetSizeFromWordWithFrequency(string word, int frequency)
    {
        return new Size(
            (10 + frequency) * word.Length,
            15 + frequency
        );
    }
}