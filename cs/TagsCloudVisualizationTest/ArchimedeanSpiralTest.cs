using System.Drawing;
using FluentAssertions;
using TagsCloudVisualization;

namespace TagsCloudVisualizationTest;

public class ArchimedeanSpiralTest
{
    private Point _center;
    
    [SetUp]
    public void SetUp()
    {
        _center = new Point(0, 0);
    }
    
    [TestCase(2, TestName = "positive integer coefficient")]
    [TestCase(1.5, TestName = "positive fractional coefficient")]
    public void Constructor_DoesNotThrowException_OnValidCoefficient(double coefficient)
    {
        var creation = () => new ArchimedeanSpiral(_center, coefficient);
        
        creation.Should().NotThrow();
    }

    [TestCase(-1, TestName = "negative coefficient")]
    [TestCase(0, TestName = "zero coefficient")]
    public void Constructor_ThrowsArgumentException_OnInvalidCoefficient(double coefficient)
    {
        var creation = () => new ArchimedeanSpiral(_center, coefficient);
        
        creation.Should().Throw<ArgumentException>();
    }

    [Test]
    public void Constructor_DoesNotThrowException_OnDefaultValues()
    {
        var creation = () => new ArchimedeanSpiral();
        
        creation.Should().NotThrow();
    }

    [Test]
    public void GetCartesianPointTest_ShouldReturnSpiralCenter_OnZeroDegrees()
    {
        var spiral = new ArchimedeanSpiral(_center);

        var point = spiral.GetCartesianPoint(0);

        point.Should().BeEquivalentTo(_center);
    }
}