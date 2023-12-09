using System.Drawing;
using FluentAssertions;
using TagsCloudVisualization;
using System.Diagnostics;
using NUnit.Framework.Interfaces;

namespace TagsCloudVisualizationTests;

public class CircularCloudLayouterTests
{
    private CircularCloudLayouter circularCloudLayouter = null!;
    private const string FailOutputName = "failFile";
    private Point center;

    [SetUp]
    public void CircularCloudLayouterSetUp()
    {
        center = new Point(960, 540);
        circularCloudLayouter = new CircularCloudLayouter(center);
    }

    [TearDown]
    public void TagCloudVisualizerCircularCloudLayouterTearDown()
    {
        if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
        {
            using var tagCloudVisualizer =
                new TagCloudVisualizer(circularCloudLayouter,
                    new ImageGenerator(FileHandler.GetRelativeFilePath($"out/{FailOutputName}.jpg"), 30, 1920, 1080),
                    new WordsDataSet("words"));
            tagCloudVisualizer.GenerateTagCloud();
            Console.WriteLine("Tag cloud visualization saved to file " +
                              FileHandler.GetRelativeFilePath($"out/{FailOutputName}.jpg"));
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
            new TagCloudVisualizer(new CircularCloudLayouter(new Point(0, 0)),
                new ImageGenerator(FileHandler.GetRelativeFilePath($"out/{timeOutputName}.jpg"), 30, 1920, 1080),
                new WordsDataSet(wordsFileName));
        tagCloudVisualizer.GenerateTagCloud();

        sw.Stop();

        return sw.Elapsed;
    }

    [Test]
    public void Rectangles_NotIntersects()
    {
        var wordsDataSet = new WordsDataSet("words");

        var allRectanglesArea = 0;
        foreach (var kvp in wordsDataSet.CreateFrequencyDict())
        {
            var rectangle = circularCloudLayouter.PutNextRectangle(new Size(45, 15));
            allRectanglesArea += rectangle.Height * rectangle.Width;
        }
        
        var cloudArea = circularCloudLayouter.PlacedRectangles.Sum(rectangle => rectangle.Height * rectangle.Width);
        
        allRectanglesArea.Should().Be(cloudArea);
    }
}