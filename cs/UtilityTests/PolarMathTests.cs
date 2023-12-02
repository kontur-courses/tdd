using System.Drawing;
using FluentAssertions;
using Utility;

namespace UtilityTests;

public class Tests
{
    [TestCase(0, 0, 0, 0)]
    [TestCase(15, 90, 0, 15)]
    [TestCase(10, 45, 7, 7)]
    [TestCase(10, 45, 7, 7)]
    [TestCase(10, 45, -3, -3, -10, -10)]
    public void ShouldReturn_CartesianCoordinate(int radius, int angle, int expectedX, int expectedY,
        int offsetX = 0, int offsetY = 0)
    {
        var offset = new Point(offsetX, offsetY);
        var actual = PolarMath.PolarToCartesian(radius, angle, offset);
        var expected = new Point(expectedX, expectedY);
        
        actual.Should().Be(expected);
    }
}