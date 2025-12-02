using SixLabors.ImageSharp.PixelFormats;
using TagCloud;
using TagCloud.Visualisation;
using Color = SixLabors.ImageSharp.Color;
using Point = System.Drawing.Point;
using Rectangle = System.Drawing.Rectangle;
using Size = System.Drawing.Size;

namespace TagsCloudVisualisationExamples;

[TestFixture]
[Explicit]
public class Examples
{
    private Point layoutCenter;
    private CircularCloudLayouter layouter;
    private const string ExamplesRepository = "Examples";

    [SetUp]
    public void SetUp()
    {
        layoutCenter = new Point(0, 0);
        layouter = new CircularCloudLayouter(layoutCenter);
    }

    [Test]
    public void WithFixedBlueRectangles()
    {
        var rectangles = GenerateDefaultRectangles(60, new Size(40, 20));
        var config = new CloudVisualizationConfigBuilder()
            .WithImageSize(800, 800)
            .WithBackground(Color.White)
            .WithCenter(new Point(400, 400))
            .WithRectangleColor(new Rgba32(100, 149, 237, 200))
            .Build();
        var visualizer = new CloudVisualizer(config);
        const string fileName = "cloud_fixed_blue.png";

        visualizer.DrawLayout(rectangles, Path.Combine(ExamplesRepository, fileName));
    }

    [Test]
    public void WithRandomRectangleColorsOnDarkBackground()
    {
        var rectangles = GenerateDefaultRectangles(200, new Size(30, 30));
        var config = new CloudVisualizationConfigBuilder()
            .WithImageSize(1000, 1000)
            .WithBackground(new Rgba32(30, 30, 30, 255))
            .WithCenter(new Point(500, 500))
            .WithRandomRectangleColors(seed: 42)
            .Build();
        var visualizer = new CloudVisualizer(config);
        const string fileName = "cloud_random_colors.png";

        visualizer.DrawLayout(rectangles, Path.Combine(ExamplesRepository, fileName));
    }

    [Test]
    public void WithShiftedCenter()
    {
        var rectangles = GenerateDefaultRectangles(50, new Size(50, 15));
        var config = new CloudVisualizationConfigBuilder()
            .WithImageSize(800, 600)
            .WithBackground(new Rgba32(250, 250, 240, 255))
            .WithCenter(new Point(250, 200))
            .WithRandomRectangleColors(seed: 7)
            .Build();
        var visualizer = new CloudVisualizer(config);
        const string fileName = "cloud_shifted_center.png";

        visualizer.DrawLayout(rectangles, Path.Combine(ExamplesRepository, fileName));
    }

    [Test]
    public void WithDifferentRectangles()
    {
        var rectangles = GenerateInvertedRectangles(60, new Size(100, 20));
        var config = new CloudVisualizationConfigBuilder()
            .WithImageSize(800, 800)
            .WithBackground(Color.White)
            .WithCenter(new Point(400, 400))
            .WithRectangleColor(new Rgba32(100, 149, 237, 200))
            .Build();
        var visualizer = new CloudVisualizer(config);
        const string fileName = "cloud_diff_rectangles.png";

        visualizer.DrawLayout(rectangles, Path.Combine(ExamplesRepository, fileName));
    }

    [Test]
    public void WithGrowingRectangles()
    {
        var rectangles = GenerateGrowingRectangles(12, new Size(10, 10), 2);
        var config = new CloudVisualizationConfigBuilder()
            .WithImageSize(8000, 8000)
            .WithBackground(Color.White)
            .WithCenter(new Point(4000, 4000))
            .WithRectangleColor(new Rgba32(100, 149, 237, 200))
            .Build();
        var visualizer = new CloudVisualizer(config);
        const string fileName = "cloud_growing_rectangles.png";

        visualizer.DrawLayout(rectangles, Path.Combine(ExamplesRepository, fileName));
    }

    #region Generators

    private IEnumerable<Rectangle> GenerateInvertedRectangles(int count, Size baseSize)
    {
        var rectangles = new List<Rectangle>();

        for (var i = 0; i < count; i++)
        {
            var size = new Size(baseSize.Width, baseSize.Height);
            if (i % 2 == 0)
                size = new Size(baseSize.Height, baseSize.Width);
            if (size.Width < 5) size.Width = 5;
            if (size.Height < 5) size.Height = 5;

            var rect = layouter.PutNextRectangle(size);
            rectangles.Add(rect);
        }

        return rectangles;
    }

    private IEnumerable<Rectangle> GenerateDefaultRectangles(int count, Size baseSize)
    {
        var rnd = new Random(123);
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

    private IEnumerable<Rectangle> GenerateGrowingRectangles(int count, Size baseSize, int growingCoeff)
    {
        var rectangles = new List<Rectangle>();

        for (var i = 0; i < count; i++)
        {
            baseSize = new Size(baseSize.Width * growingCoeff, baseSize.Height * growingCoeff);
            if (baseSize.Width < 5) baseSize.Width = 5;
            if (baseSize.Height < 5) baseSize.Height = 5;

            var rect = layouter.PutNextRectangle(baseSize);
            rectangles.Add(rect);
        }

        return rectangles;
    }

    #endregion
}