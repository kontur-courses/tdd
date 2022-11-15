using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization;

namespace TagsCloudVisualizationTest;

public class ArchimedeanSpiralTest
{
    private Point center;

    [SetUp]
    public void SetUp()
    {
        center = new Point(0, 0);
    }

    [TestCase(2, TestName = "positive integer coefficient")]
    [TestCase(1.5, TestName = "positive fractional coefficient")]
    public void Constructor_DoesNotThrowException_OnValidCoefficient(double coefficient)
    {
        var action = () => new ArchimedeanSpiral(center, coefficient);

        action.Should().NotThrow();
    }

    [TestCase(-1, TestName = "negative coefficient")]
    [TestCase(0, TestName = "zero coefficient")]
    public void Constructor_ThrowsArgumentException_OnInvalidCoefficient(double coefficient)
    {
        var action = () => new ArchimedeanSpiral(center, coefficient);

        action.Should().Throw<ArgumentException>();
    }

    [Test]
    public void Constructor_DoesNotThrowException_OnDefaultValues()
    {
        var action = () => new ArchimedeanSpiral();

        action.Should().NotThrow();
    }

    [Test]
    public void GetCartesianPoint_ShouldReturnSpiralCenter_OnZeroDegrees()
    {
        var spiral = new ArchimedeanSpiral(center);

        var point = spiral.GetCartesianPoint(0);

        point.Should().BeEquivalentTo(center);
    }
}