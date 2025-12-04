using System.Drawing;
using SkiaSharp;
using TagCloud.Creators;
using TagCloud.Shapes;
using TagCloud.SpiralGenerators;

namespace TagCloud.Visualization;

internal static class Program
{
    private static void Main()
    {
        const string imagesDirectory = "images";
        if (!Directory.Exists(imagesDirectory))
            Directory.CreateDirectory(imagesDirectory);

        GenerateCloud(
            Path.Combine(imagesDirectory, "cloud_wide_and_tall.png"),
            2000,
            2000,
            200,
            random =>
            {
                var isWide = random.NextDouble() < 0.5;
                return isWide
                    ? new Size(random.Next(60, 160), random.Next(15, 40))
                    : new Size(random.Next(15, 40), random.Next(60, 160));
            });

        GenerateCloud(
            Path.Combine(imagesDirectory, "cloud_mixed_random.png"),
            2000,
            2000,
            250,
            sizeFactory: random =>
                new Size(random.Next(20, 120), random.Next(20, 80)));

        GenerateCloud(
            Path.Combine(imagesDirectory, "cloud_extreme_shapes.png"),
            2000,
            2000,
            220,
            random =>
            {
                var mode = random.Next(0, 3);
                return mode switch
                {
                    0 => new Size(random.Next(100, 200), random.Next(10, 25)), 
                    1 => new Size(random.Next(10, 25), random.Next(100, 200)),
                    _ => new Size(random.Next(20, 60), random.Next(20, 60)) 
                };
            });
    }

    private static void GenerateCloud(
        string filePath,
        int imageWidth,
        int imageHeight,
        int rectanglesCount,
        Func<Random, Size> sizeFactory)
    {
        var center = new Point(imageWidth / 2, imageHeight / 2);
        var generator = new ArchimedeanSpiralPointGenerator(center);
        var shapeCreator = new RectangleCloudShapeCreator(); 
        var layouter = new CircularCloudLayouter(center, generator, shapeCreator);
        var random = new Random(1);

        for (var i = 0; i < rectanglesCount; i++)
        {
            var size = sizeFactory(random);
            layouter.PutNextShape(size);
        }
        CloudVisualizer.SaveCloudImage(layouter, imageWidth, imageHeight, filePath);
    }
}
