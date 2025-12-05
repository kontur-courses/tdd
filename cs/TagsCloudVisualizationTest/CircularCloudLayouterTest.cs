using System.Drawing;
using FluentAssertions;
using NUnit.Framework.Interfaces;
using TagsCloudVisualization;
using TagsCloudVisualization.CircularCloudLayouter;
using TagsCloudVisualization.PointGenerator;


namespace TagsCloudVisualizationTest;

public class CircularCloudLayouterTest
{
    private Point validCenter;
    private Size validRectangleSize;
    private IPointGenerator pointGenerator;
    private int maxPointsPerRectangle;
    private List<Rectangle> testingRectangles;
    private int countRectangles;

    [SetUp]
    public void Setup()
    {
        validCenter = new Point(1920 / 2, 1080 / 2);
        validRectangleSize = new Size(50, 30);
        pointGenerator = new SpiralPointGenerator(validCenter);
        maxPointsPerRectangle = 40000;
        countRectangles = 50;
    }

    [TearDown]
    public void TearDown()
    {
        var currentContext = TestContext.CurrentContext;
        if (currentContext.Result.Outcome.Status != TestStatus.Failed)
            return;

        var renderer = new TagCloudRenderer(new Size(1920, 1080));
        var bitmap = renderer.CreateRectangleCloud(testingRectangles);
        var fileName = $"{currentContext.Test.Name}.png";

        var imageSaver = new ImageSaver();
        imageSaver.Save(bitmap, fileName);

        Console.WriteLine($"Tag cloud visualization saved to file {fileName}");
    }

    [TestCase(-500, 500)]
    [TestCase(500, -500)]
    public void CircularCloudLayouter_ShouldThrowException_WhenInvalidCenter(int centerX, int centerY)
    {
        var invalidCenter = new Point(centerX, centerY);

        var act = () => new CircularCloudLayouter(invalidCenter, maxPointsPerRectangle, pointGenerator);

        act.Should().Throw<ArgumentException>()
            .WithMessage("Center should be greater than 0");
    }

    [TestCase(-1)]
    [TestCase(0)]
    public void CircularCloudLayouter_ShouldThrowException_WhenInvalidCount(int count)
    {
        var act = () => new CircularCloudLayouter(validCenter, count, pointGenerator);

        act.Should().Throw<ArgumentException>()
            .WithMessage("maxPointsPerRectangle must be greater than 0");
    }

    [TestCase(-100, 100)]
    [TestCase(100, -100)]
    [TestCase(0, 100)]
    [TestCase(100, 0)]
    public void PutNextRectangle_ShouldThrowException_WhenInvalidRectangleSize(int rectangleWidth, int rectangleHeight)
    {
        var circularCloud = new CircularCloudLayouter(validCenter, maxPointsPerRectangle, pointGenerator);

        var act = () => circularCloud.PutNextRectangle(new Size(rectangleWidth, rectangleHeight));

        act.Should().Throw<ArgumentException>()
            .WithMessage("Rectangle size must be positive");
    }


    [Test]
    public void PutNextRectangle_ShouldThrowException_WhenNotFoundPointInPoints()
    {
        var circularCloud = new CircularCloudLayouter(validCenter, 1, pointGenerator);

        var act1 = () => circularCloud.PutNextRectangle(validRectangleSize);
        var act2 = () => circularCloud.PutNextRectangle(validRectangleSize);

        act1.Should().NotThrow();
        act2.Should().Throw<ArgumentException>()
            .WithMessage($"Failed to find place for the 1 rectangle");
    }

    [Test]
    public void PutNextRectangle_ShouldNotProduceIntersectingRectangles_WhenCalledMultipleTimes()
    {
        var rectangles = CloudFactory
            .CreateFixedSizeRectangles(countRectangles, validCenter, validRectangleSize, pointGenerator)
            .ToList();
        testingRectangles = rectangles;

        Geometry.HasIntersectingRectangles(rectangles).Should().BeFalse();
    }


    [TestCase(0.1)]
    [TestCase(0.2)]
    [TestCase(0.5)]
    [TestCase(0.9)]
    public void PutNextRectangle_ShouldNotProduceIntersectingRectangles_WhenRandomSizedRectangles(
        double minScaleFactor)
    {
        var rectangles = CloudFactory
            .CreateRandomSizeRectangles(countRectangles, validCenter, validRectangleSize, pointGenerator,
                minScaleFactor)
            .ToList();
        testingRectangles = rectangles;

        Geometry.HasIntersectingRectangles(rectangles).Should().BeFalse();
    }

    [Test]
    public void FindMinRadius_ShouldReturnRadiusOfEnclosingCircle_WhenValidParameters()
    {
        var rectangles = CloudFactory
            .CreateRandomSizeRectangles(countRectangles, validCenter, validRectangleSize, pointGenerator)
            .ToList();
        testingRectangles = rectangles;

        var circleRadius = Geometry.FindMinRadius(rectangles, validCenter);

        Geometry.AreRectanglesInsideCircle(rectangles, validCenter, circleRadius).Should().BeTrue();
        Geometry.AreRectanglesInsideCircle(rectangles, validCenter, circleRadius - 1).Should().BeFalse();
    }

    [TestCase(0.3)]
    [TestCase(0.4)]
    [TestCase(0.6)]
    [TestCase(1.0)]
    public void PutNextRectangle_ShouldEnsurePackingDensity_WhenValidParameters(double minScaleFactor)
    {
        var rectangles = CloudFactory
            .CreateRandomSizeRectangles(150, validCenter, validRectangleSize, pointGenerator,
                minScaleFactor, 100000)
            .ToList();
        testingRectangles = rectangles;
        testingRectangles = rectangles;

        var circleRadius = Geometry.FindMinRadius(rectangles, validCenter);
        var packingDensity = Geometry.GetRectanglesPackingDensity(rectangles, circleRadius);

        packingDensity.Should().BeGreaterThan(0.5);
    }
}