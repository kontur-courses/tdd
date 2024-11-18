using System;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization.Extensions;

namespace TagsCloudVisualizationTests.Extensions;

[TestFixture]
[TestOf(typeof(RectangleExtension))]
public class RectangleExtensionTest
{

    [TestCase(1, 1)]
    [TestCase(2, 2)]
    [TestCase(3, 3)]
    [TestCase(2, 4)]
    [TestCase(4, 2)]
    public void GetDistanceToMostRemoteCorner_ShouldReturnExpectedValues(
        int width, int height)
    {
        var rectangle = new Rectangle(new Point(0, 0), new Size(width, height));
        var expectedDistance = Math.Sqrt(width * width + height * height);
        var actualDistance = rectangle.GetDistanceToMostRemoteCorner(new Point(0, 0));
        
        actualDistance.Should().Be(expectedDistance);
    }
    
    [TestCase(1, 1, 2, 2)]
    [TestCase(1, 1, 7, 7)]
    [TestCase(-1, -2, 4, 3)]
    [TestCase(3, -1, 4, 3)]
    [TestCase(-1, -1, 2, 2)]
    public void CreateRectangleInCenter_ShouldCreateExpectedRectangle
        (int x, int y, int width, int height)
    {
        var actualRectangle = new Rectangle().CreateRectangleWithCenter(
            new Point(x, y), new Size(width, height));
        var expectedRectangle = new Rectangle(
            new Point(x - width / 2, y - height / 2), new Size(width, height));
        
        actualRectangle.Should().BeEquivalentTo(expectedRectangle);
    }
}