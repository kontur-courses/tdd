using System.Drawing;

namespace TagsCloudRunner;

public static class SizesGenerator
{
    public static List<Size> GenerateRandomSizes(int count, int minWidth, int maxWidth, int minHeight, int maxHeight)
    {
        var random = new Random();
        var sizes = new List<Size>();

        for (var i = 0; i < count; i++)
        {
            sizes.Add(new Size(
                random.Next(minWidth, maxWidth),
                random.Next(minHeight, maxHeight)));
        }

        return sizes;
    }
}