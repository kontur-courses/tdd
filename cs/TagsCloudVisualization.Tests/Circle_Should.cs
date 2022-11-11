using System.Drawing;
using FluentAssertions;
using TagsCloudVisualization.Geometry;

namespace TagsCloudVisualization.Tests;

[TestFixture]
public class Circle_Should
{
    [Test]
    public void ContainsRectangle_ShouldReturnFalse_WhenRectangleOutside()
    {
        var circle = new Circle(3, new Point(0, 0));
        circle.ContainsRectangle(new Rectangle(3, 3, 1, 1)).Should().Be(false);
    }

    [Test]
    public void ContainsRectangle_ShouldReturnFalse_WhenHasBigPartOfRectangleOutside()
    {
        var circle = new Circle(3, new Point(0, 0));
        circle.ContainsRectangle(new Rectangle(0, 0, 10, 10)).Should().Be(false);
    }
    
    [Test]
    public void ContainsRectangle_ShouldWorkCorrect_WithNegativeLocation()
    {
        var circle = new Circle(9, new Point(-54, -54));
        circle.ContainsRectangle(new Rectangle(-50, -50, 1, 1)).Should().Be(true);
    }

    [TestCase(0, TestName = "zero radius")]
    [TestCase(-1, TestName = "negative radius")]
    public void ThrowException_WhenInitWithNonPositiveRadius(int radius)
    {
        var circleCreate = () => new Circle(radius, new Point(0, 0));
        circleCreate.Should().Throw<ArgumentException>();
    }
    
    [Test]
    public void ContainsRectangle_ShouldReturnFalse_WhenBigRectangleOverlapsCircle()
    {
        var circle = new Circle(3, new Point(0, 0));
        circle.ContainsRectangle(new Rectangle(-50, -50, 100, 100)).Should().Be(false);
    }
}