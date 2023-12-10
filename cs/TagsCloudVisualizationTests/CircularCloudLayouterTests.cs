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
            var rectangleSizes = new List<Size>();
            foreach (var rectangle in circularCloudLayouter.PlacedRectangles)
                rectangleSizes.Add(rectangle.Size);

            new TagCloudVisualizer(circularCloudLayouter,
                    new ImageGenerator(
                        FileHandler.GetOutputRelativeFilePath($"{FailOutputName}.jpg"),
                        FileHandler.GetSourceRelativeFilePath("JosefinSans-Regular.ttf"),
                        30, 1920, 1080))
                .GenerateLayout(rectangleSizes);
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
        var words100Time = AlgorithmTimeComplexity(100);
        var words1000Time = AlgorithmTimeComplexity(1000);

        (words1000Time.TotalNanoseconds / words100Time.TotalNanoseconds).Should().BeLessOrEqualTo(100);
    }

    private TimeSpan AlgorithmTimeComplexity(int count)
    {
        var sw = new Stopwatch();

        sw.Start();

        var tmpLayouter = new CircularCloudLayouter(center);
        
        for (var _ = 0; _ < count; _++)
            tmpLayouter.PutNextRectangle(new Size(45, 15));
        
        sw.Stop();

        return sw.Elapsed;
    }

    [Test]
    public void Rectangles_NotIntersects()
    {
        for (var _ = 0; _ < 100; _++)
            circularCloudLayouter.PutNextRectangle(new Size(45, 15));
        
        circularCloudLayouter.PlacedRectangles
            .All(rect1 => circularCloudLayouter.PlacedRectangles
                .All(rect2 => rect1 == rect2 || !rect1.IntersectsWith(rect2))).Should().BeTrue();
    }

    [Test]
    public void TagCloudIsDensityAndShapeCloseToCircleWithCenter()
    {
        for (var _ = 0; _ < 100; _++)
            circularCloudLayouter.PutNextRectangle(new Size(45, 15));

        const double densityRatio = 0.3;

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
                            rectangle.Location.Y + rectangle.Height),
                        center),
                    PointMath.DistanceToCenter(
                        new Point(
                            rectangle.Location.X + rectangle.Width,
                            rectangle.Location.Y + rectangle.Height),
                        center))
            )
        );
        var outlineCircleArea = Math.PI * maxRadius * maxRadius;

        (cloudArea / outlineCircleArea).Should().BeGreaterThan(densityRatio);
    }
}