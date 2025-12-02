using System.Drawing;
using TagCloud;
using TagCloud.Visualisation;

namespace TagCloudVisualizationExamples;

internal static class Program
{
    private static readonly Point LayoutCenter = new(0, 0);
    private const string ExamplesRepository = "Examples";

    public static void Main()
    {
        Directory.CreateDirectory(ExamplesRepository);

        WithRandomRectangleColorsOnDarkBackground();
        WithDifferentRectangles();
        WithShiftedCenter();

        Console.WriteLine("Examples generated into folder: " + ExamplesRepository);
    }

    private static void WithRandomRectangleColorsOnDarkBackground()
    {
        var rectangles = GenerateDefaultRectangles(200, new Size(30, 30));

        var config = new CloudVisualizationConfigBuilder()
            .WithImageSize(1000, 1000)
            .WithBackground(CloudColor.Black)
            .WithCenter(new Point(500, 500))
            .WithRandomRectangleColors(seed: 42)
            .Build();

        var visualizer = new CloudVisualizer(config);
        const string fileName = "cloud_random_colors.png";

        visualizer.DrawLayout(rectangles, Path.Combine(ExamplesRepository, fileName));
    }

    private static void WithShiftedCenter()
    {
        var rectangles = GenerateDefaultRectangles(50, new Size(50, 15));

        var config = new CloudVisualizationConfigBuilder()
            .WithImageSize(800, 600)
            .WithBackground(CloudColor.White)
            .WithCenter(new Point(250, 200))
            .WithRandomRectangleColors(seed: 7)
            .Build();

        var visualizer = new CloudVisualizer(config);
        const string fileName = "cloud_shifted_center.png";

        visualizer.DrawLayout(rectangles, Path.Combine(ExamplesRepository, fileName));
    }

    private static void WithDifferentRectangles()
    {
        var rectangles = GenerateInvertedRectangles(60, new Size(100, 20));

        var config = new CloudVisualizationConfigBuilder()
            .WithImageSize(800, 800)
            .WithBackground(CloudColor.White)
            .WithCenter(new Point(400, 400))
            .WithRectangleColor(CloudColor.Green)
            .Build();

        var visualizer = new CloudVisualizer(config);
        const string fileName = "cloud_diff_rectangles.png";

        visualizer.DrawLayout(rectangles, Path.Combine(ExamplesRepository, fileName));
    }

    #region Generators

    private static IEnumerable<Rectangle> GenerateInvertedRectangles(int count, Size baseSize)
    {
        var layouter = new CircularCloudLayouter(LayoutCenter);
        var rectangles = new List<Rectangle>();

        for (var i = 0; i < count; i++)
        {
            var size = (i % 2 == 0)
                ? new Size(baseSize.Height, baseSize.Width)
                : baseSize;

            if (size.Width < 5) size.Width = 5;
            if (size.Height < 5) size.Height = 5;

            var rect = layouter.PutNextRectangle(size);
            rectangles.Add(rect);
        }

        return rectangles;
    }

    private static IEnumerable<Rectangle> GenerateDefaultRectangles(int count, Size baseSize)
    {
        var rnd = new Random(123);
        var layouter = new CircularCloudLayouter(LayoutCenter);
        var rectangles = new List<Rectangle>();

        for (var i = 0; i < count; i++)
        {
            var size = new Size(
                baseSize.Width + rnd.Next(-10, 11),
                baseSize.Height + rnd.Next(-5, 6));

            if (size.Width < 5) size.Width = 5;
            if (size.Height < 5) size.Height = 5;

            var rect = layouter.PutNextRectangle(size);
            rectangles.Add(rect);
        }

        return rectangles;
    }

    #endregion
}
