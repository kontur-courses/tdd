using System.Drawing;
using CircularCloudLayouter;
using CircularCloudLayouter.WeightedLayouter;
using FluentAssertions;
using NUnit.Framework;
using static FluentAssertions.FluentActions;

namespace TagsCloudVisualization_Tests;

[TestFixture]
public class CircularCloudLayouter_Should
{
    private ICircularCloudLayouter _defaultCircularCloudLayouter = null!;
    private readonly Random _random = new();

    [SetUp]
    public void Setup()
    {
        _defaultCircularCloudLayouter = new WeightedCircularCloudLayouter(new Point(0, 0));
    }

    [TestCase(3, 4, TestName = "Usual rectangle")]
    [TestCase(500_000, 900_000, TestName = "Big rectangle")]
    public void ReturnCorrectSizeRectangle_Successful(int width, int height)
    {
        var size = new Size(width, height);
        _defaultCircularCloudLayouter
            .PutNextRectangle(new Size(width, height)).Size
            .Should().Be(size);
    }

    [TestCase(0, 0, TestName = "Zero placed rectangle")]
    [TestCase(2, 5, TestName = "Positive side placed rectangle")]
    [TestCase(-2, -5, TestName = "Negative side placed rectangle")]
    [TestCase(5_000_000, -5, TestName = "Big X value")]
    [TestCase(2, 5_000_000, TestName = "Big Y value")]
    public void ReturnCenterPlacedRectangle_WhenPutFirst(int centerX, int centerY)
    {
        var actualRect = new WeightedCircularCloudLayouter(new Point(centerX, centerY))
            .PutNextRectangle(new Size(1, 2));
        var actualCenter = actualRect.Location + actualRect.Size / 2;
        actualCenter.X.Should().BeGreaterThan(centerX - 1).And.BeLessOrEqualTo(centerX + 1);
        actualCenter.Y.Should().BeGreaterThan(centerY - 1).And.BeLessOrEqualTo(centerY + 1);
    }

    [TestCase(1, 1, 1, 1, TestName = "Small equal values")]
    [TestCase(100_000, 100_000, 100_000, 100_000, TestName = "Big equal values")]
    [TestCase(1, 1, 100_000, 100_000, TestName = "Small width and big height equal values")]
    [TestCase(100_000, 100_000, 1, 1, TestName = "Big width and small height equal values")]
    [TestCase(1, 100_000, 1, 100_000, TestName = "Different values")]
    public void ReturnNonIntersectingRects_OnRandomValues(int minWidth, int maxWidth, int minHeight, int maxHeight)
    {
        var rects = new List<Rectangle>();
        for (var i = 0; i < 1000; i++)
        {
            var size = new Size(
                _random.Next(minWidth, maxWidth + 1),
                _random.Next(minHeight, maxHeight + 1)
            );
            var newRect = _defaultCircularCloudLayouter.PutNextRectangle(size);
            rects.Any(rect => rect.IntersectsWith(newRect))
                .Should().BeFalse("rectangles should not intersects");
            rects.Add(newRect);
        }
    }

    [TestCase(0, 0, TestName = "Zero center")]
    [TestCase(0, 100, TestName = "Zero x, positive y")]
    [TestCase(0, -100, TestName = "Zero x, negative y")]
    [TestCase(100, 0, TestName = "Positive x, zero y")]
    [TestCase(-100, 0, TestName = "Negative x, zero y")]
    [TestCase(10_000_000, 10_000_000, TestName = "Big positive values")]
    [TestCase(-10_000_000, -10_000_000, TestName = "Big negative values")]
    public void ReturnNonIntersectingRects_OnAnyCenterPosition(int centerX, int centerY)
    {
        var layouter = new WeightedCircularCloudLayouter(new Point(centerX, centerY));
        var rects = new List<Rectangle>();
        for (var i = 0; i < 1000; i++)
        {
            var size = new Size(
                _random.Next(1, 100_000),
                _random.Next(1, 100_000)
            );
            var newRect = layouter.PutNextRectangle(size);
            rects.Any(rect => rect.IntersectsWith(newRect))
                .Should().BeFalse("rectangles should not intersects");
            rects.Add(newRect);
        }
    }

    [TestCase(0, 10, "width", TestName = "Zero width")]
    [TestCase(10, 0, "height", TestName = "Zero height")]
    [TestCase(0, 0, "width", "height", TestName = "Zero width, zero height")]
    public void ThrowExceptions_OnIncorrectSize(int width, int height, params string[] messageParts)
    {
        Invoking(() => _defaultCircularCloudLayouter.PutNextRectangle(new Size(width, height)))
            .Should().Throw<ArgumentException>().And.Message.ToLower()
            .Should().ContainAll(messageParts);
    }
}