using System.Drawing;
using FluentAssertions;
using Moq;
using Moq.Language.Flow;
using TagsCloudVisualization;

namespace TagsCloudVisualizationTests;

public class CircularCloudLayouterTests
{
    private ISetup<IPointGenerator, IEnumerable<Point>> pointGeneratorSetup;
    private Mock<IPointGenerator> pointGenerator;

    [SetUp]
    public void Setup()
    {
        pointGenerator = new Mock<IPointGenerator>();
        pointGeneratorSetup = pointGenerator.Setup(p => p.Generate(It.IsAny<Point>()));
    }

    [Test]
    public void PutNextRectangle_ShouldCenterRectangleOnFirstCandidate_WhenLayoutIsEmpty()
    {
        var center = new Point(100, 80);
        var size = new Size(20, 10);
        pointGeneratorSetup.Returns(new[] { center });
        var expected = new Rectangle(90, 75, 20, 10);
        var sut = new CircularCloudLayouter(center, pointGenerator.Object);

        var actual = sut.PutNextRectangle(size);

        actual.Should().Be(expected);
    }

    [Test]
    public void PutNextRectangle_ShouldSkipCandidate_WhenOverlap()
    {
        var center = new Point(100, 80);
        var size = new Size(10, 10);
        pointGeneratorSetup.Returns(new[] { center, new Point(105, 80), new Point(120, 80) });
        var expected = new Rectangle(115, 75, 10, 10);
        var sut = new CircularCloudLayouter(center, pointGenerator.Object);
        sut.PutNextRectangle(size);

        var actual = sut.PutNextRectangle(size);

        actual.Should().Be(expected);
    }

    [Test]
    public void PutNextRectangle_ShouldPutRectangle_WhenTouchEdges()
    {
        var center = new Point(100, 80);
        var size = new Size(10, 10);
        pointGeneratorSetup.Returns(new[]
        {
            center,
            new Point(110, 80)
        });
        var expected = new Rectangle(105, 75, 10, 10);
        var sut = new CircularCloudLayouter(center, pointGenerator.Object);
        sut.PutNextRectangle(size);

        var actual = sut.PutNextRectangle(size);

        actual.Should().Be(expected);
    }
}