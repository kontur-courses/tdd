using NUnit.Framework.Interfaces;
using FluentAssertions;
using System.Drawing;
using TagsCloudVisualization.CloudLayouter;
using TagsCloudVisualization.Visualizers;
using NUnit.Framework;

namespace TagsCloudVisualization.Tests.CircularCloudLayouterTests;

[TestFixture, NonParallelizable]
public class CircularCloudLayouterTests
{
    private CircularCloudLayouter cloudLayouter;
    private const int imageWidth = 1500;
    private const int imageHeight = 1500;

    [SetUp]
    public void Init()
    {
        var center = new Point(imageWidth / 2, imageHeight / 2);
        cloudLayouter = new CircularCloudLayouter(center);
        cloudLayouter.GenerateCloud(100);
    }

    [TearDown]
    public void TearDown()
    {
        if (TestContext.CurrentContext.Result.Outcome.Status != TestStatus.Failed)
            return;
        var directory = "FailedVisualisations";
        var path = Path.Combine(directory, $"{TestContext.CurrentContext.Test.Name}_visualisation.png");
        var visuliser = new SimpleCloudVisualizer();
        visuliser.CreateBitmap(cloudLayouter.GeneratedRectangles, new(imageWidth, imageHeight), path);
        Console.WriteLine($"Tag cloud visualization saved to file {path}");
    }

    [TestCase(0, 1, TestName = "WhenWidthIsZero")]
    [TestCase(1, 0, TestName = "WhenHeightIsZero")]
    [TestCase(-1, 1, TestName = "WhenWidthIsNegative")]
    [TestCase(1, -1, TestName = "WhenHeightIsNegative")]
    public void PutNextRectangle_ShouldThrowArgumentException(int width, int height)
    {
        var size = new Size(width, height);

        var action = () => cloudLayouter.PutNextRectangle(size);

        action.Should().Throw<ArgumentException>();
    }

    [Test]
    public void PutNextRectangle_FirstRectangle_ShouldBeInCenter()
    {
        cloudLayouter = new CircularCloudLayouter(cloudLayouter.Center);
        var rectangleSize = new Size(10, 10);
        var expectedRectangle = new Rectangle(
            cloudLayouter.Center.X - rectangleSize.Width / 2,
            cloudLayouter.Center.Y - rectangleSize.Height / 2,
            rectangleSize.Width,
            rectangleSize.Height
        );

        var actualRectangle = cloudLayouter.PutNextRectangle(rectangleSize);

        actualRectangle.Should().BeEquivalentTo(expectedRectangle);
    }

    [Test, Parallelizable(ParallelScope.Self)]
    [Repeat(10)]
    public void PutNextRectangle_Rectangles_ShouldNotHaveIntersects() =>
        AreRectanglesHaveIntersects(cloudLayouter.GeneratedRectangles).Should().BeFalse();

    [Test]
    [Repeat(10)]
    public void PutNextRectangle_CloudCenterMust_ShouldBeInLayoterCenter()
    {
        var maxRectangleSize = 10;
        var expectedDiscrepancy = maxRectangleSize;
        var minRectangleSize = 1;
        var center = cloudLayouter.Center;
        
        cloudLayouter.GenerateCloud(100, minRectangleSize, maxRectangleSize);

        var actualCenter = GetCenterOfAllRectangles(cloudLayouter.GeneratedRectangles);
        actualCenter.X.Should().BeInRange(center.X - expectedDiscrepancy, center.X + expectedDiscrepancy);
        actualCenter.Y.Should().BeInRange(center.Y - expectedDiscrepancy, center.Y + expectedDiscrepancy);
    }

    [Test]
    [Repeat(10)]
    public void PutNextRectangle_RectanglesDensity_ShouldBeMax()
    {
        var expectedDensity = 0.45;
        var center = cloudLayouter.Center;
        var rectangles = cloudLayouter.GeneratedRectangles;

        var rectanglesArea = rectangles.Sum(rect => rect.Width * rect.Height);
        
        var radius = GetMaxDistanceBetweenRectangleAndCenter(rectangles);
        var circleArea = Math.PI * radius * radius;
        var density = rectanglesArea / circleArea;
        density.Should().BeGreaterThanOrEqualTo(expectedDensity);
    }

    private Point GetCenterOfAllRectangles(List<Rectangle> rectangles)
    {
        var top = rectangles.Max(r => r.Top);
        var right = rectangles.Max(r => r.Right);
        var bottom = rectangles.Min(r => r.Bottom);
        var left = rectangles.Min(r => r.Left);
        var x = left + (right - left) / 2;
        var y = bottom + (top - bottom) / 2;
        return new(x, y);
    }

    private double GetMaxDistanceBetweenRectangleAndCenter(List<Rectangle> rectangles)
    {
        var center = GetCenterOfAllRectangles(rectangles);
        double maxDistance = -1;
        foreach (var rectangle in rectangles)
        {
            var corners = new Point[4]
            {
                new(rectangle.Top, rectangle.Left),
                new(rectangle.Bottom, rectangle.Left),
                new(rectangle.Top, rectangle.Right),
                new(rectangle.Bottom, rectangle.Right)
            };
            var distance = corners.Max(p => GetDistanceBetweenPoints(p, center));
            maxDistance = Math.Max(maxDistance, distance);
        }
        return maxDistance;
    }

    private static bool AreRectanglesHaveIntersects(List<Rectangle> rectangles)
    {
        for (var i = 0; i < rectangles.Count; i++)
            for (var j = i + 1; j < rectangles.Count; j++)
                if (rectangles[i].IntersectsWith(rectangles[j]))
                    return true;
        return false;
    }

    private static double GetDistanceBetweenPoints(Point point1, Point point2)
        => Math.Sqrt(Math.Pow(point1.X - point2.X, 2) + Math.Pow(point1.Y - point2.Y, 2));
}