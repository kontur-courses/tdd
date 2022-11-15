using System.Drawing;
using FluentAssertions;
using FluentAssertions.Common;
using NUnit.Framework;
using TagsCloudVisualization;

namespace TagsCloudVisualizationTest;

public class ArchimedeanSpiralIteratorTest
{
    private ArchimedeanSpiral spiral;

    [SetUp]
    public void SetUp()
    {
        spiral = new ArchimedeanSpiral();
    }

    [TestCase(-90, 1, TestName = "negative start angle")]
    [TestCase(0, 0, TestName = "zero step")]
    [TestCase(0, -1, TestName = "negative step")]
    public void Constructor_ShouldThrowArgumentException_OnInvalidParameters(int phy, int step)
    {
        var action = () => new ArchimedeanSpiralIterator(spiral, phy, step);

        action.Should().Throw<ArgumentException>();
    }

    [Test]
    public void Constructor_ShouldNotThrowException_OnDefaultParameters()
    {
        var action = () => new ArchimedeanSpiralIterator(spiral);

        action.Should().NotThrow();
    }

    [Test]
    public void Constructor_ShouldNotThrowException_OnValidParameters()
    {
        var action = () => new ArchimedeanSpiralIterator(spiral, 90, 2);

        action.Should().NotThrow();
    }

    [Test]
    public void Next_ShouldReturnDifferentPoints()
    {
        var iterator = new ArchimedeanSpiralIterator(spiral);
        var points = new List<Point>();

        for (var i = 0; i < 50; i++)
            points.Add(iterator.Next());

        points.Should().OnlyHaveUniqueItems();
    }
}