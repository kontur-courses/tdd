using System.Drawing;
using FluentAssertions;
using TagsCloudVisualization;

[TestFixture]
public class PolarMathTests
{
    [Test]
    [TestCase(0, 0, 100, 100, 100, 100, TestName = "radius_is_zero")]
    [TestCase(10, 0, 100, 100, 110, 100, TestName = "angle_is_zero")]
    [TestCase(10, Math.PI, 100, 100, 90, 100, TestName = "angle_is_180_degrees")]
    [TestCase(10, Math.PI / 2, 100, 100, 100, 110, TestName = "angle_is_90_degrees")]
    [TestCase(10, 3 * Math.PI / 2, 100, 100, 100, 90, TestName = "angle_is_270_degrees")]
    public void PolarToCartesian_WithVariousParameters_ReturnsCorrectPoint(double radius, double angle, int centerX, int centerY, 
        int expectedX, int expectedY)
    {
        var center = new Point(centerX, centerY);
        var expected = new Point(expectedX, expectedY);

        var result = PolarMath.PolarToCartesian(radius, angle, center); 
        
        result.Should().Be(expected);
    }
}