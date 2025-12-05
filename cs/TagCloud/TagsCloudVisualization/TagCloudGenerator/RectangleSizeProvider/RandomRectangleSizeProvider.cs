using System.Drawing;

namespace TagsCloudVisualization;

public class RandomRectangleSizeProvider : IRectangleSizeProvider
{
    private readonly Random _random;

    public RandomRectangleSizeProvider(Random? random = null)
    {
        _random = random ?? new Random();
    }

    public IEnumerable<Size> GetSizes(int count, Size minSize, Size maxSize)
    {
        if (count <= 0)
            throw new ArgumentException("Count must be positive", nameof(count));
        
        if (minSize.Width <= 0 || minSize.Height <= 0)
            throw new ArgumentException("Min size must have positive dimensions", nameof(minSize));
        
        if (maxSize.Width < minSize.Width || maxSize.Height < minSize.Height)
            throw new ArgumentException("Max size must be greater than or equal to min size", nameof(maxSize));

        for (var i = 0; i < count; i++)
        {
            var width = _random.Next(minSize.Width, maxSize.Width + 1);
            var height = _random.Next(minSize.Height, maxSize.Height + 1);
            yield return new Size(width, height);
        }
    }
}