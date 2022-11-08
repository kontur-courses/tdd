using System.Drawing;
using CircularCloudLayouter;
using CircularCloudLayouter.WeightedLayouter;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using static FluentAssertions.FluentActions;

namespace TagsCloudVisualization_Tests;

[TestFixture]
public class CircularCloudLayouter_Should
{
    private readonly Random _random = new();
    private ICircularCloudLayouter _defaultCircularCloudLayouter = null!;
    private List<Rectangle> _rectangles = null!;

    [SetUp]
    public void Setup()
    {
        _defaultCircularCloudLayouter = new WeightedCircularCloudLayouter(new Point(0, 0));
        _rectangles = new List<Rectangle>();
    }

    [TearDown]
    public void TearDown()
    {
        if (TestContext.CurrentContext.Result.Outcome.Status != TestStatus.Failed || _rectangles.Count == 0)
            return;
        var filePath = ErrorImageSaver.SaveErrorResult(_rectangles, TestContext.CurrentContext.Test.Name);
        TestContext.Out.WriteLine("Image with error saved to " + filePath);
    }

    [TestCase(3, 4, TestName = "Usual rectangle")]
    [TestCase(500_000, 900_000, TestName = "Big rectangle")]
    public void ReturnCorrectSizeRectangle_Successful(int width, int height)
    {
        var size = new Size(width, height);
        var rect = _defaultCircularCloudLayouter.PutNextRectangle(new Size(width, height));
        _rectangles.Add(rect);
        rect.Size.Should().Be(size);
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
        _rectangles.Add(actualRect);
        var actualCenter = actualRect.Location + actualRect.Size / 2;

        actualCenter.X.Should().BeGreaterThan(centerX - 1).And.BeLessOrEqualTo(centerX + 1);
        actualCenter.Y.Should().BeGreaterThan(centerY - 1).And.BeLessOrEqualTo(centerY + 1);
    }

    [TestCase(1, 1, TestName = "Small width and height")]
    [TestCase(1000, 1000, TestName = "Big width and height")]
    [TestCase(1, 1000, TestName = "Small width and big height")]
    [TestCase(1000, 1, TestName = "Big width and small height")]
    public void ReturnNonIntersectingRects_OnEqualsValues(int width, int height)
    {
        var size = new Size(width, height);
        for (var i = 0; i < 100; i++)
        {
            var newRect = _defaultCircularCloudLayouter.PutNextRectangle(size);
            _rectangles.Add(newRect);

            _rectangles
                .Take(_rectangles.Count - 1)
                .Any(rect => rect.IntersectsWith(newRect))
                .Should().BeFalse("rectangles should not intersects");
        }
        // true.Should().BeFalse();
    }

    [Test]
    [Repeat(10)]
    public void ReturnNonIntersectingRects_OnRandomValues()
    {
        for (var i = 0; i < 100; i++)
        {
            var size = new Size(
                _random.Next(1, 1000),
                _random.Next(1, 1000)
            );
            var newRect = _defaultCircularCloudLayouter.PutNextRectangle(size);
            _rectangles.Add(newRect);
            _rectangles
                .Take(_rectangles.Count - 1)
                .Any(rect => rect.IntersectsWith(newRect))
                .Should().BeFalse("rectangles should not intersects");
        }
    }

    [TestCase(0, 0, TestName = "Zero center")]
    [TestCase(0, 100, TestName = "Zero x, positive y")]
    [TestCase(0, -100, TestName = "Zero x, negative y")]
    [TestCase(100, 0, TestName = "Positive x, zero y")]
    [TestCase(-100, 0, TestName = "Negative x, zero y")]
    [TestCase(10_000_000, 10_000_000, TestName = "Big positive values")]
    [TestCase(-10_000_000, -10_000_000, TestName = "Big negative values")]
    [Repeat(10)]
    public void ReturnNonIntersectingRects_OnAnyCenterPosition(int centerX, int centerY)
    {
        var layouter = new WeightedCircularCloudLayouter(new Point(centerX, centerY));
        for (var i = 0; i < 100; i++)
        {
            var size = new Size(
                _random.Next(1, 1000),
                _random.Next(1, 1000)
            );
            var newRect = layouter.PutNextRectangle(size);
            _rectangles.Add(newRect);
            _rectangles
                .Take(_rectangles.Count - 1)
                .Any(rect => rect.IntersectsWith(newRect))
                .Should().BeFalse("rectangles should not intersects");
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