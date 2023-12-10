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
            new TagCloudVisualizer(circularCloudLayouter,
                    new ImageGenerator(
                        FileHandler.GetOutputRelativeFilePath($"{FailOutputName}.jpg"),
                        FileHandler.GetSourceRelativeFilePath("JosefinSans-Regular.ttf"),
                        30, 1920, 1080),
                    new MockWordsDataSet())
                .ShowTagCloudLayout();
            Console.WriteLine("Tag cloud visualization saved to file " +
                              FileHandler.GetOutputRelativeFilePath($"{FailOutputName}.jpg"));
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
        var words100Time = AlgorithmTimeComplexity("words100", "testFile");
        var words1000Time = AlgorithmTimeComplexity("words1000", "testFile");

        (words1000Time.Nanoseconds / words100Time.Nanoseconds).Should().BeLessOrEqualTo(100);
    }

    private TimeSpan AlgorithmTimeComplexity(string wordsFileName, string timeOutputName)
    {
        var sw = new Stopwatch();

        sw.Start();

        var tagCloudVisualizer =
            new TagCloudVisualizer(new CircularCloudLayouter(new Point(0, 0)),
                new ImageGenerator(
                    FileHandler.GetOutputRelativeFilePath($"{timeOutputName}.jpg"),
                    FileHandler.GetSourceRelativeFilePath("JosefinSans-Regular.ttf"),
                    30, 1920, 1080),
                new WordsDataSet(FileHandler.ReadText(wordsFileName)));
        tagCloudVisualizer.GenerateTagCloud();

        sw.Stop();

        return sw.Elapsed;
    }

    [Test]
    public void Rectangles_NotIntersects()
    {
        circularCloudLayouter.PlacedRectangles
            .All(rect1 => circularCloudLayouter.PlacedRectangles
                .All(rect2 => !rect1.IntersectsWith(rect2))).Should().BeFalse();
    }

    [Test]
    public void TagCloudIsDensityAndShapeCloseToCircleWithCenter()
    {
        new TagCloudVisualizer(circularCloudLayouter,
            new ImageGenerator(
                FileHandler.GetOutputRelativeFilePath("testFile.jpg"),
                FileHandler.GetSourceRelativeFilePath("JosefinSans-Regular.ttf"),
                30, 1920, 1080),
            new WordsDataSet(FileHandler.ReadText("words"))).GenerateTagCloud();

        const double densityRatio = 0.6;

        var cloudArea = circularCloudLayouter.PlacedRectangles.Sum(rectangle => rectangle.Height * rectangle.Width);

        var maxRadius = circularCloudLayouter.PlacedRectangles.Max(
            rectangle => Math.Max(
                Math.Max(
                    PointMath.DistanceToCenter(
                        rectangle.Location,
                        center),
                    PointMath.DistanceToCenter(
                        new Point(
                            rectangle.Location.X + rectangle.Width,
                            rectangle.Location.Y),
                        center)),
                Math.Max(
                    PointMath.DistanceToCenter(
                        new Point(
                            rectangle.Location.X,
                            rectangle.Location.Y + rectangle.Width),
                        center),
                    PointMath.DistanceToCenter(
                        new Point(
                            rectangle.Location.X + rectangle.Width,
                            rectangle.Location.Y + rectangle.Width),
                        center))
            )
        );
        var outlineCircleArea = Math.PI * maxRadius * maxRadius;

        (outlineCircleArea / cloudArea).Should().BeGreaterThan(densityRatio);
    }

    private class MockWordsDataSet() : WordsDataSet("")
    {
        public override Dictionary<string, int> CreateFrequencyDict()
        {
            var dict = new Dictionary<string, int>();

            for (var i = 0; i < 100; i++)
                dict.Add(i.ToString(), 0);

            return dict;
        }
    }
}