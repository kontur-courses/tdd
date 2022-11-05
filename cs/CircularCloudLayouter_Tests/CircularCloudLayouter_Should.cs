using System.Drawing;
using CircularCloudLayouter;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization_Tests;

[TestFixture]
public class CircularCloudLayouter_Should
{
    private ICircularCloudLayouter _defaultCircularCloudLayouter = null!;

    [SetUp]
    public void Setup()
    {
        _defaultCircularCloudLayouter = new WeightedCircularCloudLayouter(new Point(0, 0));
    }

    [TestCase(0, 0, TestName = "Zero rectangle")]
    [TestCase(3, 4, TestName = "Usual rectangle")]
    [TestCase(500000, 900000, TestName = "Big rectangle")]
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
    public void ReturnCenterPlacedRectangle_WhenCallFirstTime(int centerX, int centerY)
    {
        var center = new Point(centerX, centerY);
        var actualRect = new WeightedCircularCloudLayouter(center).PutNextRectangle(new Size(1, 2));
        var actualCenter = actualRect.Location + actualRect.Size / 2;
        actualCenter.Should().Be(center);
    }
}