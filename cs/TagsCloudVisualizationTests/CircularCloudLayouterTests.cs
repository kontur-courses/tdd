using System.Drawing;
using FluentAssertions;
using TagsCloudVisualization;
using System.Diagnostics;

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
    public void PutNextRectangle_OnEmptyLayout_Should_PlaceInCenter()
    {
        var size = new Size(10, 10);
        var actual = circularCloudLayouter.PutNextRectangle(size);
        var expected = new Rectangle(center, size);
        actual.Should().Be(expected);
    }
    
    [Test]
    public void AlgorithmTimeComplexity_LessOrEqualQuadratic()
    {
        var words100Time = AlgorithmTimeComplexity("words100", "words100");
        var words1000Time = AlgorithmTimeComplexity("words1000", "words1000");

        (words1000Time.Nanoseconds / words100Time.Nanoseconds).Should().BeLessOrEqualTo(100);
    }

    private TimeSpan AlgorithmTimeComplexity(string wordsFileName, string outputName)
    {
        var sw = new Stopwatch();
        
        sw.Start();
        
        new TagCloudVisualizer().GenerateTagCloud(new CircularCloudLayouter(), wordsFileName, outputName);
        
        sw.Stop();

        return sw.Elapsed;
    }
    
    [Test]
    public void Rectangles_NotIntersects()
    {
        new TagCloudVisualizer().GenerateTagCloud(circularCloudLayouter, outputName: OutputName);

        circularCloudLayouter.AllRectanglesArea.Should().Be(circularCloudLayouter.GetCloudArea());
    }
}