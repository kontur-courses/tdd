using System.Drawing;
using FluentAssertions;
using TagsCloudVisualization;

namespace TagsCloudVisualizationTests;

public class CircularCloudLayouterTests
{
    private CircularCloudLayouter circularCloudLayouter = null!;
    private const string OutputName = "testFile";
    private Point center;
    private Size resolution;

    [SetUp]
    public void CircularCloudLayouterSetUp()
    {
        center = new Point(960, 540);
        resolution = new Size(1920, 1080);
        circularCloudLayouter = new CircularCloudLayouter(center, resolution);
    }

    [Test]
    public void PutNextRectangle_Should_NotPlaceWord_When_SpaceOccupied()
    {
        var size = new Size(10, 10);
        var actual = circularCloudLayouter.PutNextRectangle(size);
        var expected = new Rectangle(center, size);
        actual.Should().Be(expected);
    }

    [Test]
    public void GenerateTagCloudShouldCreateFile()
    {
        new TagCloudVisualizer().GenerateTagCloud(outputName: OutputName);

        File.Exists($"../../../../TagsCloudVisualization/out/{OutputName}.jpg").Should().BeTrue();
    }

    [Test]
    public void AlgorithmComplexity_LessOrEqualQuadratic()
    {
        // TODO
    }
    
    [Test]
    public void ShapeCloseToCircleWithCenter()
    {
        // TODO
    }
    
    [Test]
    public void TagCloudIsDensity()
    {
        // TODO
    }
}