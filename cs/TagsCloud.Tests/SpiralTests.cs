using FluentAssertions;
using NUnit.Framework;
using SixLabors.ImageSharp;
using TagsCloudVisualization;

namespace TagsCloud.Tests;

[TestFixture]
public class SpiralTests
{
    [SetUp]
    public void SetUp()
    {
        spiral = new Spiral(distanceDelta, angleDelta);
    }

    private const float distanceDelta = 0.5f;
    private const float angleDelta = (float)Math.PI / 4;

    private Spiral spiral;

    [Test]
    public void Spiral_Should_SaveStateBetweenCalls()
    {
        var another = new Spiral(distanceDelta, angleDelta);
        _ = spiral.GetNextPoint();

        spiral.GetNextPoint().Should().NotBe(another.GetNextPoint());
    }

    [Test]
    public void GetNextPoint_Should_ReturnCartesianCoordinates()
    {
        // Skip first point, because it is (0, 0)
        _ = spiral.GetNextPoint();

        const float radius = distanceDelta * angleDelta;
        var expected = new PointF(radius, angleDelta);
        expected.ConvertToCartesian();

        spiral.GetNextPoint().Should().Be(expected);
    }

    [Test]
    public void GetNextPoint_Should_ReturnCorrectValues()
    {
        var angle = 0f;

        for (var i = 0; i < 1000; i++)
        {
            var expected = new PointF(distanceDelta * angle, angle);
            expected.ConvertToCartesian();

            angle += angleDelta;

            var actual = spiral.GetNextPoint();
            actual.Should().Be(expected);
        }
    }
}