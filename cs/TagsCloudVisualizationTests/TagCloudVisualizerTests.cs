using System.Drawing;
using FluentAssertions;
using TagsCloudVisualization;

namespace TagsCloudVisualizationTests;

public class TagCloudVisualizerTests
{
    private CircularCloudLayouter circularCloudLayouter = null!;
    private TagCloudVisualizer tagCloudVisualizer = null!;
    private const string OutputName = "testFile";
    private Point center;

    [SetUp]
    public void TagCloudVisualizerSetUp()
    {
        center = new Point(960, 540);
        circularCloudLayouter = new CircularCloudLayouter(center);
        tagCloudVisualizer = new TagCloudVisualizer(circularCloudLayouter, 
            new ImageGenerator(OutputName, 30, 1920, 1080),
            new WordsDataSet("words"));
    }

    [TearDown]
    public void TagCloudVisualizerTearDown()
    {
        tagCloudVisualizer.Dispose();
    }

    [Test]
    public void GenerateTagCloudShouldCreateFile()
    {
        tagCloudVisualizer.GenerateTagCloud();

        File.Exists(FileHandler.GetRelativeFilePath($"out/{OutputName}.jpg")).Should().BeTrue();
    }

    [Test]
    public void TagCloudIsDensityAndShapeCloseToCircleWithCenter()
    {
        tagCloudVisualizer.GenerateTagCloud();

        const double densityRatio = 0.6;

        var cloudArea = circularCloudLayouter.PlacedRectangles.Sum(rectangle => rectangle.Height * rectangle.Width);
        
        var maxRadius = circularCloudLayouter.PlacedRectangles.Max(
            rectangle => PointMath.DistanceToCenter(rectangle.Location, center));
        var outlineCircleArea = Math.PI * maxRadius * maxRadius;

        (outlineCircleArea / cloudArea).Should().BeGreaterThan(densityRatio);
    }
}