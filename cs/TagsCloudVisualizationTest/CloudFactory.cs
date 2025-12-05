using System.Drawing;
using TagsCloudVisualization.CircularCloudLayouter;
using TagsCloudVisualization.PointGenerator;

namespace TagsCloudVisualizationTest;

public static class CloudFactory
{
    public static IEnumerable<Rectangle> CreateFixedSizeRectangles(int count, Point center, Size rectangleSize,
        IPointGenerator pointGenerator, int maxPointsPerRectangle = 10000)
    {
        if (count <= 0)
            throw new ArgumentException("Count must be positive");
        
        var layouter = new CircularCloudLayouter(center, maxPointsPerRectangle, pointGenerator);

        for (var i = 0; i < count; i++)
            yield return layouter.PutNextRectangle(rectangleSize);
    }

    public static IEnumerable<Rectangle> CreateRandomSizeRectangles(int count, Point center, Size rectangleSize,
        IPointGenerator pointGenerator, double minScaleFactor = 0.4, int maxPointsPerRectangle = 10000)
    {
        if (count <= 0)
            throw new ArgumentException("Count must be positive");
        
        var layouter = new CircularCloudLayouter(center, maxPointsPerRectangle, pointGenerator);
        var random = new Random(42);
        for (var i = 0; i < count; i++)
        {
            var rnd = random.NextDouble() + minScaleFactor;
            var rndWidth = (int)Math.Round(rectangleSize.Width * rnd);
            var rndHeight = (int)Math.Round(rectangleSize.Height * rnd);
            yield return layouter.PutNextRectangle(new Size(rndWidth, rndHeight));
        }
    }
}