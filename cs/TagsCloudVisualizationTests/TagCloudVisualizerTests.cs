using System.Drawing;
using FluentAssertions;
using TagsCloudVisualization;

namespace TagsCloudVisualizationTests;

public class TagCloudVisualizerTests
{
    private CircularCloudLayouter circularCloudLayouter = null!;
    private TagCloudVisualizer tagCloudVisualizer = null!;
    private const string OutputName = "testFile";

    [SetUp]
    public void TagCloudVisualizerSetUp()
    {
        var center = new Point(960, 540);
        var resolution = new Size(1920, 1080);
        circularCloudLayouter = new CircularCloudLayouter(center, resolution);
        tagCloudVisualizer = new TagCloudVisualizer();
    }

    [Test]
    public void GenerateTagCloudShouldCreateFile()
    { 
        tagCloudVisualizer.GenerateTagCloud(circularCloudLayouter, outputName: OutputName);

        File.Exists($"../../../../TagsCloudVisualization/out/{OutputName}.jpg").Should().BeTrue();
    }
    
    [Test]
    public void TagCloudIsDensityAndShapeCloseToCircleWithCenter()
    {
        tagCloudVisualizer.GenerateTagCloud(circularCloudLayouter, outputName: OutputName);

        const double densityRatio = 0.6;
        
        var cloudArea = circularCloudLayouter.GetCloudArea();
        var outlineCircleArea = 
            Math.PI * circularCloudLayouter.MaxRadius * circularCloudLayouter.MaxRadius;

        (outlineCircleArea / cloudArea).Should().BeGreaterThan(densityRatio);
    }
}