using System.Drawing;
using TagsCloudCore.CloudLayout;
using TagsCloudCore.CloudVisualization;

namespace TagsCloudRunner;

internal static class Program
{
    public static void Main()
    {
        GenerateTagsCloud(
            fileName: "cloud_small.png",
            center: new Point(0, 0),
            rectanglesCount: 150);

        GenerateTagsCloud(
            fileName: "cloud_medium.png",
            center: new Point(0, 0),
            rectanglesCount: 750);

        GenerateTagsCloud(
            fileName: "cloud_big.png",
            center: new Point(0, 0),
            rectanglesCount: 1500);
    }

    private static void GenerateTagsCloud(string fileName, Point center, int rectanglesCount)
    {
        var layouter = new CircularCloudLayouter(center);

        var sizes = SizesGenerator.GenerateRandomSizes(rectanglesCount, 20, 100, 15, 40);

        foreach (var size in sizes)
        {
            layouter.PutNextRectangle(size);
        }

        var filePath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Images", fileName);
        CloudVisualizer.SaveLayoutToFile(filePath, layouter.PlacedRectangles.ToArray());
    }
}