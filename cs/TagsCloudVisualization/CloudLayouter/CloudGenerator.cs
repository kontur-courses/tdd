using System.Drawing;

namespace TagsCloudVisualization;

public static class CloudGenerator
{
    public static ICloudLayouter GenerateRandomCloudWithCenter(Point center, int rectangleCount)
    {
        var random = new Random();
        var layouter = new CircularCloudLayouter(center);

        for (var i = 0; i < rectangleCount; i++)
        {
            var width = random.NextInt64(10, 70);
            var height = random.NextInt64(10, 70);
            layouter.PutNextRectangle(new((int)width, (int)height));
        }

        return layouter;
    }
}
