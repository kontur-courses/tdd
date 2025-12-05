using System.Drawing;
using FluentAssertions;
using TagsCloudVisualization.PointGenerator;

namespace TagsCloudVisualizationTest;

public class SpiralPointGeneratorTest
{
    private Point validCenter;

    [SetUp]
    public void Setup()
    {
        validCenter = new Point(1920 / 2, 1080 / 2);
    }


    [TestCase(-1, 100)]
    [TestCase(100, -1)]
    public void PointGenerator_ShouldThrowException_WhenInvalidCenter(int x, int y)
    {
        var invalidCenter = new Point(x, y);
        var act = () => new SpiralPointGenerator(invalidCenter);

        act.Should().Throw<ArgumentException>()
            .WithMessage("Center coordinates must be non-negative");
    }

    [TestCase(-1)]
    [TestCase(0)]
    public void PointGenerator_ShouldThrowException_WhenInvalidRadius(int radius)
    {
        var act = () => new SpiralPointGenerator(validCenter, radius);

        act.Should().Throw<ArgumentException>()
            .WithMessage("Radius must be positive");
    }

    [Test]
    public void GetPointsOnSpiral_ShouldEachNextPointFartherFromCenter_WhenValidParameters()
    {
        var pointGenerator = new SpiralPointGenerator(validCenter, 50, Math.PI / 12);
        var actualPoints = pointGenerator
            .GetPoints()
            .Take(100);

        actualPoints.Should().HaveCountGreaterThan(1);

        var prevDistance = Geometry.GetDistance(actualPoints.First(), validCenter);
        foreach (var point in actualPoints.Skip(1))
        {
            var currentDistance = Geometry.GetDistance(point, validCenter);
            currentDistance.Should().BeGreaterThanOrEqualTo(prevDistance);
            prevDistance = currentDistance;
        }
    }

    [Test]
    public void GetPointsOnSpiral_ShouldAngleGrowMonotonically_WhenValidParameters()
    {
        var pointGenerator = new SpiralPointGenerator(validCenter, 50, Math.PI / 12);
        var actualPoints = pointGenerator
            .GetPoints()
            .Take(100);

        actualPoints.Should().HaveCountGreaterThan(1);

        var prevAngle = Geometry.GetAngle(actualPoints.First(), validCenter);
        foreach (var point in actualPoints.Skip(1))
        {
            var currentAngle = Geometry.GetAngle(point, validCenter);
            var delta = Geometry.GetNormalizedAngleDifference(currentAngle, prevAngle);
            delta.Should().BeGreaterThan(-1e-5);
            prevAngle = currentAngle;
        }
    }
}