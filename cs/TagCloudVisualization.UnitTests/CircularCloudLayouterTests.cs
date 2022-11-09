using System.Drawing;
using FluentAssertions;
using TagsCloudVisualization;

namespace TagCloudVisualization.UnitTests;

[TestFixture]
public class CircularCloudLayouterTests
{
    [SetUp]
    public void SetUp()
    {
        layouter = new(center);
    }

    private CircularCloudLayouter layouter = null!;
    private readonly Point center = new(250, 250);

    [TestCase(-1, -1)]
    [TestCase(-1, 10)]
    [TestCase(10, -1)]
    [TestCase(0, 0)]
    public void Constructor_Throws(int centerX, int centerY)
    {
        var action = () => { _ = new CircularCloudLayouter(new(centerX, centerY)); };

        action.Should().Throw<ArgumentException>();
    }

    [Test]
    public void PutNextRectangle_InvalidOperationException_OverflowedByRectangles()
    {
        var rectangleSize = new Size(500, 500);
        var action = () => { _ = layouter.PutNextRectangle(rectangleSize); };

        action.Should().NotThrow();
        action.Should().Throw<InvalidOperationException>();
    }

    [Test]
    public void PutNextRectangle_ArgumentException_HugeSize()
    {
        var rectangleSize = new Size(501, 501);
        var action = () => { _ = layouter.PutNextRectangle(rectangleSize); };

        action.Should().Throw<ArgumentException>();
    }

    [Test]
    public void PutNextRectangle_RectangleAtCenter_ValidSize()
    {
        var rectangleSize = new Size(250, 250);
        var expectedRectangle = new Rectangle(125, 125, 250, 250);

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


    [Test]
    public void PutNextRectangle_SeveralRectangleThatAreTouchesButAreNotIntersects_SeveralValidSizes()
    {
        var sizes = new[]
        {
            new Size(10, 10),
            new Size(20, 20),
            new Size(30, 30),
            new Size(10, 10),
            new Size(20, 20),
            new Size(30, 30),
            new Size(10, 10),
            new Size(20, 20),
            new Size(30, 30)
        };

        var rectangles = sizes.Select(layouter.PutNextRectangle).ToArray();

        rectangles.Distinct().Should().HaveSameCount(rectangles);
        foreach (var rectangle in rectangles)
        {
            rectangles
                .Where(other => other != rectangle)
                .Should()
                .Match(otherRectangles => otherRectangles.All(other => !other.IntersectsWith(rectangle)));

            rectangles
                .Where(other => other != rectangle)
                .Should()
                .Match(otherRectangles => otherRectangles.Any(other => other.TouchesWith(rectangle)));
        }
    }
}