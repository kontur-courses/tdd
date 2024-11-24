using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization;

namespace TagsCloudTests;

public class RectangleFExtensionsTests
{
    [TestCase(0, 0)]
    [TestCase(-5, 5)]
    [TestCase(41, -123)]
    [TestCase(-12, -3)]
    public void GetCenter_ShouldReturn_CorrectCenter(float expectedX, float expectedY)
    {
        var rectSize = new SizeF(10, 10);
        var rect = new RectangleF(expectedX - rectSize.Width / 2, expectedY + rectSize.Height / 2, rectSize.Width,
            rectSize.Height);
        rect.GetCenter().Should().Be(new PointF(expectedX, expectedY));
    }

    [TestCase(0, 0)]
    [TestCase(-5, 5)]
    [TestCase(41, -123)]
    [TestCase(-12, -3)]
    public void GetRectangleFWithCenterIn_ShouldReturn_CorrectRectangle(float expectedX, float expectedY)
    {
        var rectSize = new SizeF(10, 10);
        var rect = RectangleFExtensions.GetRectangleFWithCenterIn(new PointF(expectedX, expectedY), rectSize);
        var actualCenter = new PointF(rect.X + rect.Width / 2, rect.Y - rect.Height / 2);
        actualCenter.Should().Be(new PointF(expectedX, expectedY));
    }
}