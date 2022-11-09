using System.Drawing;
using FluentAssertions;
using TagsCloudVisualization;

namespace TagCloudVisualization.UnitTests;

[TestFixture]
public class CircularCloudLayouterTests
{
    private CircularCloudLayouter layouter = null!;
    private Point center = new Point(250, 250);

    [SetUp]
    public void SetUp()
    {
        layouter = new CircularCloudLayouter(center);
    }

    [TestCase(-1, -1)]
    [TestCase(-1, 10)]
    [TestCase(10, -1)]
    [TestCase(0, 0)]
    public void Constructor_Throws(int centerX, int centerY)
    {
        var action = () => { _ = new CircularCloudLayouter(new Point(centerX, centerY)); };

        action.Should().Throw<ArgumentException>();
    }

    [Test]
    public void PutNextRectangle_ArgumentException_HugeSize()
    {
        var rectangleSize = new Size(500, 500);
        var action = () => { _ = layouter.PutNextRectangle(rectangleSize); };

        action.Should().Throw<ArgumentException>();
    }

    [Test]
    public void PutNextRectangle_RectangleAtCenter_ValidSize()
    {
        var rectangleSize = new Size(150, 150);
        var expectedRectangle = new Rectangle(100, 100, 150, 150);

        var actualRectangle = layouter.PutNextRectangle(rectangleSize);

        actualRectangle.Should().BeEquivalentTo(expectedRectangle);
    }

    [Test]
    public void PutNextRectangle_TwoRectangleThatAreTouchesButAreNotIntersects_TwoValidSizes()
    {
        var firstRectangleSize = new Size(10, 10);
        var secondRectangleSize = new Size(10, 10);

        var actualFirstRectangle = layouter.PutNextRectangle(firstRectangleSize);
        var actualSecondRectangle = layouter.PutNextRectangle(secondRectangleSize);

        actualFirstRectangle.TouchesWith(actualSecondRectangle).Should().BeTrue();
    }
}