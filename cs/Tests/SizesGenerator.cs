using System.Drawing;

namespace Tests;

public static class SizesGenerator
{
    public static IEnumerable<Size> Generate(
        int count, int minWidthSize, int maxWidthSize, int minHeightSize, int maxHeightSize, int seed)
    {
        var random = new Random(seed);
        for (var i = 0; i < count; i++)
        {
            var width = random.Next(minWidthSize, maxWidthSize);
            var height = random.Next(minHeightSize, maxHeightSize);
            yield return new Size(width, height);
        }
    }

    public static IEnumerable<Size> GenerateSquares(int count, int seed)
    {
        return Generate(count, 10, 10, 10, 10, seed);
    }

    public static IEnumerable<Size> GenerateTallRectangles(int count, int seed)
    {
        return Generate(count, 1, 10, 50, 100, seed);
    }

    public static IEnumerable<Size> GenerateLongRectangles(int count, int seed)
    {
        return Generate(count, 50, 100, 1, 10, seed);
    }

    public static IEnumerable<Size> GenerateRandomRectangles(int count, int seed)
    {
        return Generate(count, 1, 100, 1, 10, seed);
    }

    public static IEnumerable<Size> GenerateBigRectangles(int count, int seed)
    {
        return Generate(count, 1000, 2000, 1000, 2000, seed);
    }
}