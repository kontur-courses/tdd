using System.Drawing;
using FluentAssertions;
using TagsCloudCore.CloudLayout;
using TagsCloudCore.CloudLayout.Extensions;
using TagsCloudCore.CloudVisualization;
using TagsCloudRunner;

namespace TagsCloudTests;

public class CircularCloudLayouterTests
{
    private CircularCloudLayouter layouter;

    [SetUp]
    public void Setup()
    {
        layouter = new CircularCloudLayouter(new Point(0, 0));
    }

    [TearDown]
    public void TearDown()
    {
        var result = TestContext.CurrentContext.Result.Outcome.Status;

        if (result != NUnit.Framework.Interfaces.TestStatus.Failed) return;
        var filePath = $"{TestContext.CurrentContext.Test.Name}_failed.png";

        try
        {
            CloudVisualizer.SaveLayoutToFile(filePath, layouter.PlacedRectangles.ToArray());
            TestContext.WriteLine($"Tag cloud visualization saved to file {filePath}");
        }
        catch (Exception e)
        {
            TestContext.WriteLine($"Failed to save tag cloud visualization: {e.Message}");
        }
    }

    [Test]
    public void CircularCloudLayouter_ShouldPlaceFirstRectangleCenterExactlyAtCloudCenter_Test()
    {
        var size = new Size(40, 20);

        var rectangle = layouter.PutNextRectangle(size);

        var rectangleCenter = new Point(
            rectangle.X + rectangle.Width / 2,
            rectangle.Y + rectangle.Height / 2);

        rectangleCenter.Should().Be(layouter.Center);
    }

    [Test]
    public void CircularCloudLayouter_ShouldNotProduceOverlappingRectangles_WhenPlacingMany_Test()
    {
        var sizes = SizesGenerator.GenerateRandomSizes(80, 10, 60, 10, 40);

        var rectangles = new List<Rectangle>();

        foreach (var size in sizes)
        {
            var rect = layouter.PutNextRectangle(size);
            rectangles.Add(rect);
        }

        var hasOverlap = false;

        for (var i = 0; i < rectangles.Count; i++)
        {
            for (var j = i + 1; j < rectangles.Count; j++)
            {
                if (rectangles[i].IntersectsWith(rectangles[j]))
                {
                    hasOverlap = true;
                    break;
                }
            }

            if (hasOverlap)
                break;
        }

        hasOverlap.Should().BeFalse();
    }


    [TestCase(-1, 10)]
    [TestCase(0, 10)]
    [TestCase(10, -1)]
    [TestCase(10, 0)]
    public void CircularCloudLayouter_ShouldThrow_WhenSizeIsInvalid_Test(int width, int height)
    {
        Action act = () => layouter.PutNextRectangle(new Size(width, height));

        act.Should()
            .Throw<ArgumentException>()
            .WithMessage("Size must be positive (Parameter 'rectangleSize')");
    }

    [Test]
    public void CircularCloudLayouter_ShouldReturnReasonablyCompactCloud_Test()
    {
        var sizes = SizesGenerator.GenerateRandomSizes(200, 10, 40, 10, 30);

        var rectangles = new List<Rectangle>();

        foreach (var size in sizes)
        {
            var rect = layouter.PutNextRectangle(size);
            rectangles.Add(rect);
        }

        var totalArea = rectangles.Sum(r => r.Width * r.Height);

        var maxRadius = rectangles.Max(r => r.GetDistanceToPoint(layouter.Center));

        var circleArea = Math.PI * maxRadius * maxRadius;
        var density = totalArea / circleArea;

        density.Should().BeGreaterThan(0.20);
    }


    [Test]
    public void CircularCloudLayouter_ShouldFormRoughCircularShape_Test()
    {
        var sizes = SizesGenerator.GenerateRandomSizes(150, 10, 50, 10, 35);

        var rectangles = new List<Rectangle>();

        foreach (var size in sizes)
        {
            var rect = layouter.PutNextRectangle(size);
            rectangles.Add(rect);
        }

        var distances = rectangles
            .Select(r => r.GetDistanceToPoint(layouter.Center))
            .ToArray();

        var averageDistance = distances.Average();
        var variance = distances.Select(d => Math.Pow(d - averageDistance, 2)).Average();
        var standardDeviation = Math.Sqrt(variance);

        var variationCoefficient = standardDeviation / averageDistance;
        (variationCoefficient).Should().BeLessThan(0.45);
    }
}