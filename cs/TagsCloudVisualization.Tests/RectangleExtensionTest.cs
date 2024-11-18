using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization.Extension;

namespace TagsCloudVisualizationTests;

[TestFixture]
public class RectangleExtensionTest
{
    [TestCase(0, 0, 10, 12, 5, 6, Description = "Rectangle with zero location")]
    [TestCase(5, 6, 10, 12, 10, 12, Description = "Rectangle with positive location")]
    [TestCase(-5, -6, 10, 12, 0, 0, Description = "Rectangle with negative location")]
    [TestCase(0, 0, 3, 2, 1, 1, Description = "Rectangle with odd width")]
    [TestCase(0, 0, 2, 3, 1, 1, Description = "Rectangle with odd height")]
    public void GetCenter_ReturnsCenterOfRectangle(int x, int y, int width, int height, int xCenter, int yCenter)
    {
        var rectangle = new Rectangle(x, y, width, height);
        var center = rectangle.GetCenter();
        center.X.Should().Be(xCenter);
        center.Y.Should().Be(yCenter);
    }

    [TestCase(5, 5, 10, 10, Description = "Rectangles intersect")]
    [TestCase(0, 0, 10, 10, Description = "Rectangles are equal")]
    [TestCase(0, 0, 5, 5, Description = "Rectangle is inside another rectangle")]
    public void IsIntersectOthersRectangles_WhenRectanglesIntersect(int x, int y, int width, int height)
    {
        var rectangle = new Rectangle(0, 0, 10, 10);
        var rectangles = new List<Rectangle> { new Rectangle(x, y, width, height) };
        rectangle.IntersectsWithAnyOf(rectangles).Should().BeTrue();
    }

    [TestCase(0, 1, 1, 1, Description = "The rectangle touch at the top")]
    [TestCase(1, 0, 1, 1, Description = "The rectangle touch on the right side")]
    [TestCase(0, -1, 1, 1, Description = "The rectangle touch at the bottom")]
    [TestCase(-1, 0, 1, 1, Description = "The rectangle touch on the left side")]
    [TestCase(1, 1, 1, 1, Description = "The rectangle touch at the top right corner")]
    [TestCase(-1, 1, 1, 1, Description = "The rectangle touch at the top left corner")]
    [TestCase(1, -1, 1, 1, Description = "The rectangle touch at the bottom right corner")]
    [TestCase(-1, -1, 1, 1, Description = "The rectangle touch at the bottom left corner")]
    public void IsIntersectOthersRectangles_WhenRectanglesNotIntersect(int x, int y, int width, int height)
    {
        var rectangle = new Rectangle(0, 0, 1, 1);
        var rectangles = new List<Rectangle> { new Rectangle(x, y, width, height) };
        rectangle.IntersectsWithAnyOf(rectangles).Should().BeFalse();
    }
}