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

    private static IEnumerable<TestCaseData> PutNextRectangle_PrintImage_PresentationProxy_Source()
    {
        yield return new TestCaseData(0, 80, new Size(100, 25), new Size(50, 20));
        yield return new TestCaseData(20021011, 80, new Size(100, 25), new Size(50, 20));
        yield return new TestCaseData(20221109, 80, new Size(100, 25), new Size(50, 20));
        yield return new TestCaseData(1, 1000, new Size(10, 10), new Size(10, 10));
    }

    [TestCaseSource(nameof(PutNextRectangle_PrintImage_PresentationProxy_Source))]
    public void PutNextRectangle_PrintImage_PresentationProxy(int randomSeed, int count, Size maxSize, Size minSize)
    {
        var proxy = new CircularCloudLayouterPresentationProxy(center);
        var random = new Random(randomSeed);
        var sizes = Enumerable.Range(0, count)
            .Select(_ =>
                new Size(random.Next(minSize.Width, maxSize.Width + 1),
                    random.Next(minSize.Height, maxSize.Height + 1)))
            .ToArray();

        _ = sizes.Select(proxy.PutNextRectangle).ToArray();
        proxy.SaveToFile($"test-{minSize}-{maxSize}-{randomSeed}-{count}.png");
    }
}