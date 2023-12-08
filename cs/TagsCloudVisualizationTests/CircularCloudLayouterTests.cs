using System.Drawing;
using FluentAssertions;
using TagsCloudVisualization;
using System.Diagnostics;
using NUnit.Framework.Interfaces;

namespace TagsCloudVisualizationTests;

public class CircularCloudLayouterTests
{
    private CircularCloudLayouter circularCloudLayouter = null!;
    private const string OutputName = "testFile";
    private const string FailOutputName = "failFile";
    private Point center;
    private Size resolution;

    [SetUp]
    public void CircularCloudLayouterSetUp()
    {
        center = new Point(960, 540);
        resolution = new Size(1920, 1080);
        circularCloudLayouter = new CircularCloudLayouter(center, resolution);
    }

    [TearDown]
    public void TagCloudVisualizerCircularCloudLayouterTearDown()
    {
        if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
        {
            using var tagCloudVisualizer =
                new TagCloudVisualizer(circularCloudLayouter, new ImageGenerator(FailOutputName));
            tagCloudVisualizer.GenerateTagCloud();
            Console.WriteLine("Tag cloud visualization saved to file " +
                              $"../../../../TagsCloudVisualization/out/{FailOutputName}.jpg");
        }
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

    private TimeSpan AlgorithmTimeComplexity(string wordsFileName, string timeOutputName)
    {
        var sw = new Stopwatch();

        sw.Start();

        using var tagCloudVisualizer =
            new TagCloudVisualizer(new CircularCloudLayouter(), new ImageGenerator(timeOutputName));
        tagCloudVisualizer.GenerateTagCloud(wordsFileName);

        sw.Stop();

        return sw.Elapsed;
    }

    [Test]
    public void Rectangles_NotIntersects()
    {
        using var tagCloudVisualizer = 
            new TagCloudVisualizer(circularCloudLayouter, new ImageGenerator(OutputName));
        tagCloudVisualizer.GenerateTagCloud();

        circularCloudLayouter.AllRectanglesArea.Should().Be(circularCloudLayouter.GetCloudArea());
    }
}