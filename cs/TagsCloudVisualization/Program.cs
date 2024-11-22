using SkiaSharp;
using TagsCloudVisualization.Layouter;
using TagsCloudVisualization.Renderer;

internal class Program
{
    private static void Main()
    {
        Directory.CreateDirectory("results");

        RenderCloud(GenerateRandomCloud(10), "results/cloud_10.png");
        RenderCloud(GenerateRandomCloud(50), "results/cloud_50.png");
        RenderCloud(GenerateRandomCloud(100), "results/cloud_100.png");
    }

    private static SKRect[] GenerateRandomCloud(int count)
    {
        var layouter = new CircularCloudLayouter(new SKPoint(500, 500));
        var rectangleSizes = Enumerable.Range(0, count)
            .Select(_ => new SKSize(new Random().Next(10, 100), new Random().Next(10, 100)));
        return rectangleSizes.Select(layouter.PutNextRectangle).ToArray();
    }

    private static void RenderCloud(SKRect[] rectangles, string path)
    {
        var renderer = new Renderer(new SKSize(1000, 1000));
        renderer.DrawRectangles(rectangles);
        var image = renderer.GetEncodedImage();
        using var stream = File.OpenWrite(path);
        image.SaveTo(stream);
    }
}