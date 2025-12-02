using System.Drawing;
using SixLabors.ImageSharp.PixelFormats;
using TagCloud;
using TagCloud.Visualisation;
using Color = SixLabors.ImageSharp.Color;

namespace TagsCloudVisualisationExamples;

public class Program
{
    private Point layoutCenter;
    private CircularCloudLayouter layouter;
    private const string ExamplesRepository = "Examples";

    public void Main()
    {
        layoutCenter = new Point(0, 0);
        layouter = new CircularCloudLayouter(layoutCenter);
        
        WithRandomRectangleColorsOnDarkBackground();
        WithDifferentRectangles();
        WithShiftedCenter();

    }

    private void WithRandomRectangleColorsOnDarkBackground()
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

    private void WithDifferentRectangles()
    {
        var rectangles = GenerateInvertedRectangles(60, new Size(100, 20));
        var config = new CloudVisualizationConfigBuilder()
            .WithImageSize(800, 800)
            .WithBackground(SixLabors.ImageSharp.Color.White)
            .WithCenter(new Point(400, 400))
            .WithRectangleColor(new Rgba32(100, 149, 237, 200))
            .Build();
        var visualizer = new CloudVisualizer(config);
        const string fileName = "cloud_diff_rectangles.png";

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

    #endregion
}