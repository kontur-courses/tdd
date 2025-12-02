using FluentAssertions;
using SixLabors.ImageSharp.PixelFormats;
using TagCloud;
using TagCloud.Visualisation;
using Color = SixLabors.ImageSharp.Color;
using Point = System.Drawing.Point;
using Rectangle = System.Drawing.Rectangle;
using Size = System.Drawing.Size;

namespace TagCloudTests;

[TestFixture]
[Explicit]
public class CloudVisualizerTests
{
    private Point layoutCenter;
    private CircularCloudLayouter layouter;

    [SetUp]
    public void SetUp()
    {
        layoutCenter = new Point(0, 0);
        layouter = new CircularCloudLayouter(layoutCenter);
    }

    [Test]
    public void ConfigIsNull_ShouldThrowArgumentNullException()
    {
        CloudVisualizationConfig? config = null;
        
        var act = () => new CloudVisualizer(config);
        
        act.Should().Throw<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'config')");
    }

    [Test]
    public void DrawLayout_RectanglesIsEmpty_ShouldThrowArgumentException()
    {
        var rectangles = new List<Rectangle>();
        var config = new CloudVisualizationConfigBuilder()
            .WithImageSize(800, 800)
            .WithBackground(CloudColor.White)
            .WithCenter(new Point(400, 400))
            .WithRectangleColor(CloudColor.Blue) 
            .Build();
        var visualizer = new CloudVisualizer(config);
        const string filePath = "cloud_empty.png";
        
        var act = () => visualizer.DrawLayout(rectangles, filePath);
        
        act.Should().Throw<ArgumentException>()
            .WithMessage("Коллекция прямоугольников пуста (Parameter 'rectangles')");
    }
    
    [Test]
    [TestCase(null)]
    [TestCase("")]
    [TestCase(" ")]
    public void DrawLayout_FilePathIsEmpty_ShouldThrowArgumentException(string? filePath)
    {
        var rectangles = new List<Rectangle>();
        var config = new CloudVisualizationConfigBuilder()
            .WithImageSize(800, 800)
            .WithBackground(CloudColor.White)
            .WithCenter(new Point(400, 400))
            .WithRectangleColor(CloudColor.Red) 
            .Build();
        var visualizer = new CloudVisualizer(config);
        
        var act = () => visualizer.DrawLayout(rectangles, filePath);
        
        act.Should().Throw<ArgumentException>()
            .WithMessage("Некорректное имя файла (Parameter 'fileName')");
    }

    [Test]
    public void DrawLayout_WithFixedBlueRectangles_ShouldCorrectSaveImage()
    {
        var rectangles = GenerateDefaultRectangles(60, new Size(40, 20));
        var config = new CloudVisualizationConfigBuilder()
            .WithImageSize(800, 800)
            .WithBackground(CloudColor.White)
            .WithCenter(new Point(400, 400))
            .WithRectangleColor(CloudColor.Blue) 
            .Build();
        var visualizer = new CloudVisualizer(config);
        const string fileName = "cloud_fixed_blue.png";
            
        visualizer.DrawLayout(rectangles, fileName);

        File.Exists(GetProjectPath(fileName)).Should().BeTrue();
    }

    [Test]
    public void DrawLayout_WithRandomRectangleColorsOnDarkBackground_ShouldCorrectSaveImage()
    {
        var rectangles = GenerateDefaultRectangles(80, new Size(30, 30));
        var config = new CloudVisualizationConfigBuilder()
            .WithImageSize(1000, 1000)
            .WithBackground(CloudColor.White) 
            .WithCenter(new Point(500, 500))
            .WithRandomRectangleColors(seed: 42)
            .Build();
        var visualizer = new CloudVisualizer(config);
        const string fileName = "cloud_random_colors.png";
        
        visualizer.DrawLayout(rectangles, fileName);

        File.Exists(GetProjectPath(fileName)).Should().BeTrue();
    }

    [Test]
    public void DrawLayout_WithShiftedCenter_ShouldCorrectSaveImage()
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
        
        visualizer.DrawLayout(rectangles, fileName);

        File.Exists(GetProjectPath(fileName)).Should().BeTrue();
    }
    
    [Test]
    public void DrawLayout_WithDifferentRectangles_ShouldCorrectSaveImage()
    {
        var rectangles = GenerateInvertedRectangles(60, new Size(100, 20));
        var config = new CloudVisualizationConfigBuilder()
            .WithImageSize(800, 800)
            .WithBackground(CloudColor.White)
            .WithCenter(new Point(400, 400))
            .WithRectangleColor(CloudColor.Red) 
            .Build();
        var visualizer = new CloudVisualizer(config);
        const string fileName = "cloud_diff_rectangles.png";
            
        visualizer.DrawLayout(rectangles, fileName);

        File.Exists(GetProjectPath(fileName)).Should().BeTrue();
    }
    
    [Test]
    public void DrawLayout_WithGrowingRectangles_ShouldCorrectSaveImage()
    {
        var rectangles = GenerateGrowingRectangles(12, new Size(10, 10), 2);
        var config = new CloudVisualizationConfigBuilder()
            .WithImageSize(8000, 8000)
            .WithBackground(CloudColor.White)
            .WithCenter(new Point(4000, 4000))
            .WithRectangleColor(CloudColor.Red) 
            .Build();
        var visualizer = new CloudVisualizer(config);
        const string fileName = "cloud_growing_rectangles.png";
        
        visualizer.DrawLayout(rectangles, fileName);

        File.Exists(GetProjectPath(fileName)).Should().BeTrue();
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
    
    private static string GetProjectPath(string fileName)
    {
        var projectRoot = Path.GetFullPath(
            Path.Combine(AppContext.BaseDirectory, "..", "..", ".."));
        return Path.Combine(projectRoot, fileName);
    }
}
