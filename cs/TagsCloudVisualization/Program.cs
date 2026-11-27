using System.Drawing;
using TagsCloudVisualization.Layout;
using TagsCloudVisualization.Visualization;

namespace TagsCloudVisualization;

public static class Program
{
    public static void Main()
    {
        const int rectanglesCount = 500;

        var directory = Path.GetFullPath(
            Path.Combine(AppContext.BaseDirectory,
                "..", "..", "..", "ImageExamples"));

        Directory.CreateDirectory(directory);

        var random = new Random(42);

        SaveCloud(
            Enumerable.Repeat(new Size(30, 30), rectanglesCount),
            Path.Combine(directory, "EqualSquares.png"));

        SaveCloud(
            Enumerable.Range(0, rectanglesCount)
                .Select(_ => new Size(
                    random.Next(10, 100),
                    random.Next(10, 100))),
            Path.Combine(directory, "RandomRectangles.png"));

        SaveCloud(
            Enumerable.Range(0, rectanglesCount)
                .Select(_ => new Size(
                    random.Next(30, 150),
                    random.Next(10, 30))),
            Path.Combine(directory, "HorizontalRectangles.png"));
    }

    private static void SaveCloud(IEnumerable<Size> sizes, string path)
    {
        var center = Point.Empty;
        var compactor = new RectangleCompactor();
        var spiral = new ArchimedeanSpiral(center);

        var layouter = new CircularCloudLayouter(center, spiral, compactor);

        foreach (var size in sizes)
            layouter.PutNextRectangle(size);

        CloudVisualizer.SaveImage(layouter.Rectangles, path);

        Console.WriteLine($"Saved: {path}");
    }
}
