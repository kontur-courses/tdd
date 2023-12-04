using System.Drawing;
using FluentAssertions;
using tagsCloud;

namespace TagsCloudTests;

[TestFixture]
public class SpiralTests
{
    private Point center;

    [SetUp]
    public void Setup()
    {
        center = new Point(10, 10);
    }

    [Test]
    public void Spiral_StepAngleEquals0_ShouldBeThrowException()
    {
        Action action = () => new Spiral(center, 0);
        action.Should().Throw<ArgumentException>()
            .WithMessage("angle step non be 0");
    }

    [Test]
    public void Spiral_IsNotStaticParams_ShouldBeTrue()
    {
        var spiral = new Spiral(center);
        var start = spiral.GetPoint();
        var spiral2 = new Spiral(new Point(0, 0));
        var start2 = spiral2.GetPoint();
        start.Should().NotBe(start2);
    }

    [TestCase(0.5f,
        new[] { 10, 10, 10, 10, 10, 9 },
        new[] { 10, 10, 10, 11, 12, 13 },
        TestName = "AngleStep is positive")]
    [TestCase(-0.5f,
        new[] { 10, 10, 10, 10, 10, 9 },
        new[] { 10, 10, 10, 9, 8, 7 },
        TestName = "AngleStep is negative")]
    public void Spiral_GetNextPoint_CreatePointsWithCustomAngle_ReturnsCorrectPoints(float angleStep, int[] x, int[] y)
    {
        var spiral = new Spiral(new Point(10, 10), angleStep);
        var expectedPoints = new Point[x.Length];
        var resultPoints = new Point[x.Length];
        for (var i = 0; i < x.Length; i++)
        {
            expectedPoints[i] = new Point(x[i], y[i]);
            resultPoints[i] = spiral.GetPoint();
        }

        resultPoints.Should().BeEquivalentTo(expectedPoints);
    }
}