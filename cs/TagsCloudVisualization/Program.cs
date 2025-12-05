using System.Drawing;
using TagsCloudVisualization.PointGenerator;

namespace TagsCloudVisualization;

public static class Program
{
    private static void Main()
    {
        var imageWidth = 2500;
        var imageHeight = 2500;
        var imageCenter = new Point(imageWidth / 2, imageHeight / 2);
        var rectangleSize = new Size(200, 80);
        var imageSize = new Size(imageWidth, imageHeight);

        var rectangles = CreateRandomSizeRectangles(150, imageCenter, rectangleSize);
        var cloudRenderer = new TagCloudRenderer(imageSize);
        var imageSaver = new ImageSaver();

        var image = cloudRenderer.CreateRectangleCloud(rectangles);
        imageSaver.Save(image, "cloud.png");
    }

    private static IEnumerable<Rectangle> CreateRandomSizeRectangles(int count, Point center, Size rectangleSize,
        double minScaleFactor = 0.4, int maxPointsPerRectangle = 100000)
    {
        var pointGenerator = new SpiralPointGenerator(center);
        var layouter = new CircularCloudLayouter.CircularCloudLayouter(center, maxPointsPerRectangle, pointGenerator);
        var random = new Random(10);
        for (var i = 0; i < count; i++)
        {
            var rnd = random.NextDouble() + minScaleFactor;
            var rndWidth = (int)Math.Round(rectangleSize.Width * rnd);
            var rndHeight = (int)Math.Round(rectangleSize.Height * rnd);
            yield return layouter.PutNextRectangle(new Size(rndWidth, rndHeight));
        }
    }
}