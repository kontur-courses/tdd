using FluentAssertions;
using NUnit.Framework;
using SixLabors.ImageSharp;

namespace TagsCloudTests;

public class ArchimedeanSpiralPointGenerator_Should : CircularCloudLayouterTestsBase
{
    [Test]
    public void GetNextPoint_ShouldReturnFirstPointAtCenter()
    {
        var point = Generator.GetNextPoint();
        point.Should().Be(Center);
    }

    [Test]
    public void GetNextPoint_ShouldReturnDifferentPoints()
    {
        var point1 = Generator.GetNextPoint();
        var point2 = Generator.GetNextPoint();
        point1.Should().NotBe(point2);
    }

    [Test]
    public void GetNextPoint_ShouldGeneratePointsInSpiralPattern()
    {
        var previousPoint = Generator.GetNextPoint();
        var previousDistance = Distance(Center, previousPoint);

        for (int i = 0; i < 10; i++)
        {
            var currentPoint = Generator.GetNextPoint();
            var currentDistance = Distance(Center, currentPoint);
            currentDistance.Should().BeGreaterThan(previousDistance);
            previousDistance = currentDistance;
        }
    }

    private float Distance(PointF p1, PointF p2)
    {
        return (float)Math.Sqrt(Math.Pow(p2.X - p1.X, 2) + Math.Pow(p2.Y - p1.Y, 2));
    }
}