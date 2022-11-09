using System.Drawing;
using FluentAssertions;
using TagsCloudVisualization;

namespace TagsCloudVisualizationTest;

public class ArchimedeanSpiralIteratorTest
{
    private ArchimedeanSpiral _spiral;

    [SetUp]
    public void SetUp()
    {
        _spiral = new ArchimedeanSpiral();
    }

    [TestCase(-90, 1, TestName = "negative start angle")]
    [TestCase(0, 0, TestName = "zero step")]
    [TestCase(0, -1, TestName = "negative step")]
    public void Constructor_ShouldThrowArgumentException_OnInvalidParameters(int phy, int step)
    {
        var creation = () => new ArchimedeanSpiralIterator(_spiral, phy, step);

        creation.Should().Throw<ArgumentException>();
    }

    [Test]
    public void Constructor_ShouldNotThrowException_OnDefaultParameters()
    {
        var creation = () => new ArchimedeanSpiralIterator(_spiral);

        creation.Should().NotThrow();
    }

    [Test]
    public void Constructor_ShouldNotThrowException_OnValidParameters()
    {
        var creation = () => new ArchimedeanSpiralIterator(_spiral, 90, 2);

        creation.Should().NotThrow();
    }

    [Test]
    public void Next_ShouldReturnDifferentPoints()
    {
        var iterator = new ArchimedeanSpiralIterator(_spiral);
        var points = new List<Point>();

        for (var i = 0; i < 50; i++)
            points.Add(iterator.Next());

        points.Any(point => points.Where(p => p != point)
            .Any(otherPoint => otherPoint.Equals(point))).Should().BeFalse();
    }
}