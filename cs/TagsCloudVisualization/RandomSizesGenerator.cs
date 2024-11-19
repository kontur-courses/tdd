using System.Drawing;
using TagsCloudVisualization.Interfaces;

namespace TagsCloudVisualization;

public class RandomSizesGenerator : ISizesGenerator
{
    public IEnumerable<Size> GenerateSize()
    {
        var random = new Random();
        while (true)
        {
            var rectangleWidth = random.Next(10, 100);
            var rectangleHeight = random.Next(1, 25);
            yield return new Size(rectangleWidth, rectangleHeight);
        }
        // ReSharper disable once IteratorNeverReturns
    }
}